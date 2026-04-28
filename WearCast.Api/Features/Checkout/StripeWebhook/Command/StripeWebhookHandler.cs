using WearCast.Api.Abstractions;
using WearCast.Api.Common.Enums;
using WearCast.Api.Common.Tracking;
using WearCast.Api.Common.Tracking.Models;
using WearCast.Api.Features.Checkout.StripeWebhook.DTOs;
using WearCast.Api.Persistence;
using WearCast.Api.Entities.Shipping;
using WearCast.Api.Entities.BusinessActors;
using WearCast.Api.Entities.DesignedProducts;
using Microsoft.EntityFrameworkCore;

namespace WearCast.Api.Features.Checkout.StripeWebhook.Command;

public class StripeWebhookHandler(ApplicationDbContext dbContext, ITrackingService trackingService) : IRequestHandler<StripeWebhookRequestDto, Result<bool>>
{
    public async Task<Result<bool>> Handle(StripeWebhookRequestDto request, CancellationToken cancellationToken)
    {
        var orders = await dbContext.Orders
            .Include(o => o.FixedProductItems)
            .Include(o => o.DesignedProductItems)
            .Where(o => o.StripeSessionId == request.StripeSessionId)
            .ToListAsync(cancellationToken);

        if (!orders.Any())
            return Result.Failure<bool>(new Error("Orders.NotFound", $"No orders found for session {request.StripeSessionId}", Microsoft.AspNetCore.Http.StatusCodes.Status404NotFound));

        if (!request.IsSuccess)
        {
            var existingFixedItems = orders.SelectMany(o => o.FixedProductItems).ToList();
            if (existingFixedItems.Any())
                dbContext.FixedProductOrderItems.RemoveRange(existingFixedItems);

            var existingDesignedItems = orders.SelectMany(o => o.DesignedProductItems).ToList();
            if (existingDesignedItems.Any())
                dbContext.CustomerDesignedOrderItems.RemoveRange(existingDesignedItems);

            dbContext.Orders.RemoveRange(orders);
            await dbContext.SaveChangesAsync(cancellationToken);
            return Result<bool>.Success(true);
        }

        foreach (var order in orders)
        {
            if (order.Status == OrderStatus.Paid) continue;
            
            order.Status = OrderStatus.Paid;

            // Decrement quantities for fixed product items
            foreach (var item in order.FixedProductItems)
            {
                var color = await dbContext.FixedProductColors
                    .Include(c => c.Sizes)
                    .FirstOrDefaultAsync(c => c.Id == item.FixedColorId, cancellationToken);
                
                if (color != null)
                {
                    var sizeDetail = color.Sizes.FirstOrDefault(s => s.Size.ToString() == item.SizeName);
                    if (sizeDetail != null && sizeDetail.Quantity >= item.Quantity)
                    {
                        sizeDetail.Quantity -= item.Quantity;
                    }

                    // Force EF to re-serialize the JSON-owned Sizes collection
                    dbContext.Entry(color).State = EntityState.Modified;
                }
            }

            // Increment sales count for designed product items
            foreach (var item in order.DesignedProductItems)
            {
                var customerDesign = await dbContext.CustomerDesigns
                    .FirstOrDefaultAsync(cd => cd.Id == item.CustomerDesignId, cancellationToken);

                if (customerDesign != null)
                {
                    var designedProduct = await dbContext.DesignedProducts
                        .FirstOrDefaultAsync(dp => dp.Id == customerDesign.DesignedProductId, cancellationToken);

                    if (designedProduct != null)
                    {
                        designedProduct.SalesCount += item.Quantity;
                    }
                }
            }
        }

        var customerId = orders.First().CustomerId;

        // Track purchase event
        var purchaseEvent = new PurchaseEvent
        {
            UserId = customerId.ToString(),
            Products = new List<PurchaseProductDetails>()
        };

        foreach (var order in orders.Where(o => o.Status == OrderStatus.Paid))
        {
            foreach (var item in order.FixedProductItems)
            {
                var fixedProduct = await dbContext.FixedProducts
                    .Where(p => p.Colors.Any(c => c.Id == item.FixedColorId))
                    .Include(p => p.Category)
                    .FirstOrDefaultAsync(cancellationToken);

                purchaseEvent.Products.Add(new PurchaseProductDetails
                {
                    ProductId = item.FixedColorId.ToString(),
                    Quantity = item.Quantity,
                    Price = item.UnitPrice,
                    TargetAudience = fixedProduct?.TargetAudience.ToString().Split(", ").ToList() ?? new(),
                    DressStyle = fixedProduct?.DressStyle.ToString(),
                    CategoryName = fixedProduct?.Category?.Name,
                    SellerId = fixedProduct?.SellerId
                });
            }

            foreach (var item in order.DesignedProductItems)
            {
                var designedProduct = await dbContext.DesignedProducts
                    .Include(dp => dp.Category)
                    .FirstOrDefaultAsync(dp => dp.Id == item.DesignedProductId, cancellationToken);

                purchaseEvent.Products.Add(new PurchaseProductDetails
                {
                    ProductId = item.CustomerDesignId.ToString(),
                    Quantity = item.Quantity,
                    Price = item.UnitPrice,
                    TargetAudience = designedProduct?.TargetAudience.ToString().Split(", ").ToList() ?? new(),
                    DressStyle = designedProduct?.DressStyle.ToString(),
                    CategoryName = designedProduct?.Category?.Name,
                    SellerId = null
                });
            }
        }

        trackingService.TrackPurchase(purchaseEvent);

        // Clear both fixed and designed cart items
        var cartItemsToClear = await dbContext.CartItems
            .Where(c => c.CustomerId == customerId)
            .ToListAsync(cancellationToken);
        
        dbContext.CartItems.RemoveRange(cartItemsToClear);
        
        // Find a default shipping company
        var shippingCompany = await dbContext.ShippingCompanies
            .Where(sc=>sc.IsDeleted==false).
            FirstOrDefaultAsync(cancellationToken);
        if (shippingCompany != null)
        {
            var firstOrder = orders.First();
            
            var deliveryAddress = new Common.ValueObjects.Address();
            if (firstOrder.ShippingAddress != null)
            {
                deliveryAddress.State = firstOrder.ShippingAddress.State;
                deliveryAddress.City = firstOrder.ShippingAddress.City;
                deliveryAddress.Street = firstOrder.ShippingAddress.Street;
                deliveryAddress.BuildingNumber = firstOrder.ShippingAddress.BuildingNumber;
            }

            var paidOrders = orders.Where(o => o.Status == OrderStatus.Paid).ToList();
            var ordersTotal = paidOrders.Sum(o => o.TotalAmount);
            string generatedDeliveryCode = Random.Shared.Next(100000, 999999).ToString();

            var shipment = new Shipment
            {
                CustomerId = firstOrder.CustomerId,
                DeliveryAddress = deliveryAddress,
                ShipmentStatus = ShipmentStatus.Unassigned,
                ShippingCompanyId = shippingCompany.Id,
                CreatedById = firstOrder.CreatedById,
                Price = ordersTotal + shippingCompany.DeliveryFee,
                Orders = paidOrders,
                DeliveryCode = generatedDeliveryCode
            };
            
            dbContext.Shipments.Add(shipment);
        }
        else
        {
            return Result.Failure<bool>(new Error("ShippingCompany.NotFound", "No shipping company found.", Microsoft.AspNetCore.Http.StatusCodes.Status404NotFound));
        }

        await dbContext.SaveChangesAsync(cancellationToken);
        return Result<bool>.Success(true);
    }
}
