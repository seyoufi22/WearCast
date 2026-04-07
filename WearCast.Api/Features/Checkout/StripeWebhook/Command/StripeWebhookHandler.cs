using WearCast.Api.Abstractions;
using WearCast.Api.Common.Enums;
using WearCast.Api.Features.Checkout.StripeWebhook.DTOs;
using WearCast.Api.Persistence;
using WearCast.Api.Entities.Shipping;
using WearCast.Api.Entities.BusinessActors;
using Microsoft.EntityFrameworkCore;

namespace WearCast.Api.Features.Checkout.StripeWebhook.Command;

public class StripeWebhookHandler(ApplicationDbContext dbContext) : IRequestHandler<StripeWebhookRequestDto, Result<bool>>
{
    public async Task<Result<bool>> Handle(StripeWebhookRequestDto request, CancellationToken cancellationToken)
    {
        var orders = await dbContext.Orders
            .Include(o => o.FixedProductItems)
            .Where(o => o.StripeSessionId == request.StripeSessionId)
            .ToListAsync(cancellationToken);

        if (!orders.Any())
            return Result.Failure<bool>(new Error("Orders.NotFound", $"No orders found for session {request.StripeSessionId}", Microsoft.AspNetCore.Http.StatusCodes.Status404NotFound));

        if (!request.IsSuccess)
        {
            var existingItems = orders.SelectMany(o => o.FixedProductItems).ToList();
            if (existingItems.Any())
            {
                dbContext.FixedProductOrderItems.RemoveRange(existingItems);
            }
            dbContext.Orders.RemoveRange(orders);
            await dbContext.SaveChangesAsync(cancellationToken);
            return Result<bool>.Success(true);
        }

        foreach (var order in orders)
        {
            if (order.Status == OrderStatus.Paid) continue;
            
            order.Status = OrderStatus.Paid;

            // Decrement quantities now that payment is successful
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
        }

        var customerId = orders.First().CustomerId;
        var cartItemsToClear = await dbContext.CartItems
            .Where(c => c.CustomerId == customerId && c.FixedColorId != null)
            .ToListAsync(cancellationToken);
        
        dbContext.CartItems.RemoveRange(cartItemsToClear);

        var customerId = orders.First().CustomerId;
        var cartItemsToClear = await dbContext.CartItems
            .Where(c => c.CustomerId == customerId && c.FixedColorId != null)
            .ToListAsync(cancellationToken);
        
        dbContext.CartItems.RemoveRange(cartItemsToClear);

        // Find a default shipping company
        var shippingCompany = await dbContext.ShippingCompanies.FirstOrDefaultAsync(cancellationToken);
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

            var shipment = new Shipment
            {
                CustomerID = firstOrder.CustomerId,
                DeliveryAddress = deliveryAddress,
                ShipmentStatus = ShipmentStatus.UnAssigned,
                ShippingCompanyId = shippingCompany.Id,
                CreatedById = firstOrder.CreatedById, // Inherit user mapping
                Orders = orders.Where(o => o.Status == OrderStatus.Paid).ToList()
            };
            
            // Re-fetch customer to ensure tracking or use CustomerID mapping, EF takes care of it natively
            dbContext.Shipments.Add(shipment);
        }

        await dbContext.SaveChangesAsync(cancellationToken);
        return Result<bool>.Success(true);
    }
}





