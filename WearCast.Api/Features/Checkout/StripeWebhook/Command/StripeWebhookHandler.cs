using WearCast.Api.Abstractions;
using WearCast.Api.Common.Consts;
using WearCast.Api.Common.Enums;
using WearCast.Api.Common.Tracking;
using WearCast.Api.Common.Tracking.Models;
using WearCast.Api.Common.Wallet;
using WearCast.Api.Features.Checkout.StripeWebhook.DTOs;
using WearCast.Api.Features.Checkout.StripeWebhook.Notifications;
using WearCast.Api.Persistence;
using WearCast.Api.Entities.Shipping;
using WearCast.Api.Entities.BusinessActors;
using WearCast.Api.Entities.DesignedProducts;
using Microsoft.EntityFrameworkCore;

namespace WearCast.Api.Features.Checkout.StripeWebhook.Command;

public class StripeWebhookHandler(ApplicationDbContext dbContext, ITrackingService trackingService, IWalletService walletService, IMediator mediator) : IRequestHandler<StripeWebhookRequestDto, Result<bool>>
{
    public async Task<Result<bool>> Handle(StripeWebhookRequestDto request, CancellationToken cancellationToken)
    {
        var orders = await dbContext.Orders
            .Include(o => o.FixedProductItems)
            .Include(o => o.DesignedProductItems)
            .Include(o => o.Seller).ThenInclude(s => s.Managers)
            .Include(o => o.Factory).ThenInclude(f => f.Managers)
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

        // Get platform commission rate
        var platformSettings = await dbContext.PlatformSettings.FirstOrDefaultAsync(cancellationToken);
        var commissionRate = platformSettings?.CommissionPercentage ?? 2m;

        foreach (var order in orders)
        {
            if (order.Status == OrderStatus.Paid) continue;
            
            order.Status = OrderStatus.Paid;

            // Calculate commission and payout
            order.Commission = Math.Round(order.TotalAmount * commissionRate / 100, 2);
            order.Payout = order.TotalAmount - order.Commission;

            // Credit seller or factory wallet (thread-safe)
            if (order.SellerId.HasValue)
            {
                await walletService.CreditAsync(WalletOwnerType.Seller, order.SellerId.Value, order.Payout, $"Payout for order #{order.Id}", order.Id, order.CreatedById, cancellationToken);
            }
            else if (order.FactoryId.HasValue)
            {
                await walletService.CreditAsync(WalletOwnerType.Factory, order.FactoryId.Value, order.Payout, $"Payout for order #{order.Id}", order.Id, order.CreatedById, cancellationToken);
            }

            // Credit WearCast's wallet with commission (thread-safe)
            await walletService.CreditAsync(WalletOwnerType.Platform, 1, order.Commission, $"Commission for order #{order.Id}", order.Id, order.CreatedById, cancellationToken);

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

                    if (color.ProductId > 0)
                    {
                        await dbContext.FixedProducts
                            .Where(p => p.Id == color.ProductId)
                            .ExecuteUpdateAsync(s => s.SetProperty(p => p.SalesCount, p => p.SalesCount + item.Quantity), cancellationToken);
                    }
                }
            }

            // Increment sales count for designed product items
            foreach (var item in order.DesignedProductItems)
            {
                var customerDesign = await dbContext.CustomerDesigns
                    .FirstOrDefaultAsync(cd => cd.Id == item.CustomerDesignId, cancellationToken);

                if (customerDesign != null)
                {
                    if (customerDesign.DesignedProductId > 0)
                    {
                        await dbContext.DesignedProducts
                            .Where(dp => dp.Id == customerDesign.DesignedProductId)
                            .ExecuteUpdateAsync(s => s.SetProperty(dp => dp.SalesCount, dp => dp.SalesCount + item.Quantity), cancellationToken);
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
            .Include(sc => sc.Managers)
            .Where(sc=>sc.IsDeleted==false).
            FirstOrDefaultAsync(cancellationToken);
        if (shippingCompany != null)
        {
            var firstOrder = orders.First();
            
            var deliveryAddress = new WearCast.Api.Common.ValueObjects.Address();
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
                ShipmentStatus = ShipmentStatus.Pending,
                ShippingCompanyId = shippingCompany.Id,
                CreatedById = firstOrder.CreatedById,
                Price = ordersTotal + shippingCompany.DeliveryFee,
                Orders = paidOrders,
                DeliveryCode = generatedDeliveryCode
            };
            
            dbContext.Shipments.Add(shipment);


            var ordersBySeller = paidOrders.Where(o => o.SellerId.HasValue).GroupBy(o => o.SellerId);
            foreach (var sellerGroup in ordersBySeller)
            {
                if (sellerGroup.Key.HasValue)
                {
                    var seller = sellerGroup.First().Seller;
                    if (seller != null && seller.Managers.Any())
                    {
                        var managerUserIds = seller.Managers.Where(m => !m.IsDeleted).Select(m => m.UserId).ToList();
                        foreach (var order in sellerGroup)
                        {
                            var notificationEvent = new NewOrderEvent(
                                RecipientIds: managerUserIds,
                                OrderId: order.Id,
                                PayoutAmount: order.Payout);
                            await mediator.Publish(notificationEvent, cancellationToken);
                        }
                    }
                }
            }

            var factoryOrder = paidOrders.FirstOrDefault(o => o.FactoryId.HasValue);
            if (factoryOrder != null)
            {
                var factory = factoryOrder.Factory;
                if (factory != null && factory.Managers.Any())
                {
                    var managerUserIds = factory.Managers.Where(m => !m.IsDeleted).Select(m => m.UserId).ToList();
                    var notificationEvent = new NewOrderEvent(
                        RecipientIds: managerUserIds,
                        OrderId: factoryOrder.Id,
                        PayoutAmount: factoryOrder.Payout);
                    await mediator.Publish(notificationEvent, cancellationToken);
                }
            }

            List<string> ShipmentReceipnts = new List<string>();            
            if (shippingCompany.Managers.Any())
            {
                var shippingCompanyManagerUserIds = shippingCompany.Managers.Where(m => !m.IsDeleted).Select(m => m.UserId).ToList();
                ShipmentReceipnts.AddRange(shippingCompanyManagerUserIds);
            }

            var Admins = await (from user in dbContext.Users
                                                join ur in dbContext.UserRoles on user.Id equals ur.UserId
                                                join r in dbContext.Roles on ur.RoleId equals r.Id
                                                where (r.Name == DefaultRoles.OperationsAdmin || r.Name == DefaultRoles.CustomerServiceAdmin) && !user.IsDeleted
                                                select user.Id).ToListAsync(cancellationToken);
            ShipmentReceipnts.AddRange(Admins);
             var shipmentNotificationEvent = new NewShipmentEvent(
                RecipientIds: ShipmentReceipnts,
                ShipmentId: shipment.Id,
                OrderCount: paidOrders.Count,
                DeliveryFee: shippingCompany.DeliveryFee);
             await mediator.Publish(shipmentNotificationEvent, cancellationToken);

        }
        else
        {
            return Result.Failure<bool>(new Error("ShippingCompany.NotFound", "No shipping company found.", Microsoft.AspNetCore.Http.StatusCodes.Status404NotFound));
        }

        await dbContext.SaveChangesAsync(cancellationToken);

        // Credit the shipping company's wallet AFTER SaveChanges so shipment.Id is the real DB-generated ID
        if (shippingCompany != null)
        {
            await walletService.CreditAsync(
                WalletOwnerType.ShippingCompany,
                shippingCompany.Id,
                shippingCompany.DeliveryFee,
                $"Delivery fee for shipment #{shipment!.Id}",
                firstOrder!.Id,
                firstOrder.CreatedById,
                cancellationToken);
        }

        return Result<bool>.Success(true);
    }
}
