using WearCast.Api.Features.Orders.UpdateOrderStatus.DTOs;
using WearCast.Api.Features.Shipments.Driver.UpdateShipmentStatus;

namespace WearCast.Api.Features.Orders.UpdateOrderStatus.Command;

public class UpdateOrderStatusHandler(ApplicationDbContext dbContext, IMediator _mediator)
    : IRequestHandler<UpdateOrderStatusCommand, Result<bool>>
{
    public async Task<Result<bool>> Handle(UpdateOrderStatusCommand request, CancellationToken cancellationToken)
    {
        var order = await dbContext.Orders
            .FirstOrDefaultAsync(o => o.Id == request.OrderId && !o.IsDeleted, cancellationToken);

        if (order is null)
            return Result.Failure<bool>(new Error("Orders.NotFound", "Order not found.", StatusCodes.Status404NotFound));

        // --- Role-based transition guards ---

        // Seller: can only mark Paid -> Ready (order must belong to their store)
        if (request.SellerId.HasValue)
        {
            if (order.SellerId != request.SellerId.Value)
                return Result.Failure<bool>(new Error("Orders.Forbidden", "You do not own this order.", StatusCodes.Status403Forbidden));

            if (request.NewStatus != OrderStatus.Ready)
                return Result.Failure<bool>(new Error("Orders.InvalidTransition",
                    "Sellers can only mark orders as Ready.", StatusCodes.Status400BadRequest));

            if (order.Status != OrderStatus.Paid)
                return Result.Failure<bool>(new Error("Orders.InvalidTransition",
                    $"Cannot change status from {order.Status} to Ready. Order must be Paid first.", StatusCodes.Status400BadRequest));
        }

        // Factory: can only mark Paid -> Ready (order must belong to their factory)
        if (request.FactoryId.HasValue)
        {
            if (order.FactoryId != request.FactoryId.Value)
                return Result.Failure<bool>(new Error("Orders.Forbidden", "You do not own this order.", StatusCodes.Status403Forbidden));

            if (request.NewStatus != OrderStatus.Ready)
                return Result.Failure<bool>(new Error("Orders.InvalidTransition",
                    "Factories can only mark orders as Ready.", StatusCodes.Status400BadRequest));

            if (order.Status != OrderStatus.Paid)
                return Result.Failure<bool>(new Error("Orders.InvalidTransition",
                    $"Cannot change status from {order.Status} to Ready. Order must be Paid first.", StatusCodes.Status400BadRequest));
        }

        // Driver: can only mark Ready -> PickedUp
        if (request.DriverId.HasValue || request.IsShippingCompanyManager)
        {
            if (!request.IsShippingCompanyManager)
            {
                bool isAssigned = await dbContext.Shipments
                    .AnyAsync(s => s.Id == order.ShipmentId && s.DriverId == request.DriverId.Value, cancellationToken);

                if (!isAssigned)
                {
                    return Result.Failure<bool>(AuthErrors.Forbidden);
                }
            }

            if (request.NewStatus != OrderStatus.PickedUp)
                return Result.Failure<bool>(new Error("Orders.InvalidTransition",
                    "Drivers can only mark orders as PickedUp.", StatusCodes.Status400BadRequest));

            if (order.Status != OrderStatus.Ready)
                return Result.Failure<bool>(new Error("Orders.InvalidTransition",
                    $"Cannot change status from {order.Status} to PickedUp. Order must be Ready first.", StatusCodes.Status400BadRequest));
        }

        order.Status = request.NewStatus;
        await dbContext.SaveChangesAsync(cancellationToken);

        if (request.NewStatus == OrderStatus.Ready)
        {
            bool hasNotReadyOrders = await dbContext.Orders
                .AnyAsync(o => o.ShipmentId == order.ShipmentId
                     && o.Id != order.Id
                     && o.Status != OrderStatus.Ready, cancellationToken);

            if (!hasNotReadyOrders)
            {
                await dbContext.Shipments
                    .Where(s => s.Id == order.ShipmentId)
                    .ExecuteUpdateAsync(setters => setters
                    .SetProperty(s => s.ShipmentStatus, ShipmentStatus.Unassigned)
                    .SetProperty(s => s.ReadyForPickupAt, DateTime.UtcNow),
                    cancellationToken);

                var recipients = await dbContext.ShippingCompanyManagers
                    .Where(m => !m.IsDeleted)
                    .Select(m => m.UserId)
                    .ToListAsync(cancellationToken);

                var customerUserId = await dbContext.Customers
                    .Where(c => c.Id == order.CustomerId)
                    .Select(c => c.UserId)
                    .FirstOrDefaultAsync(cancellationToken);

                if (!string.IsNullOrEmpty(customerUserId))
                {
                    recipients.Add(customerUserId);
                }

                var notificationEvent = new ShipmentReadyEvent(
                    RecipientIds: recipients,
                    ShipmentId: order.ShipmentId.Value
                );
                await _mediator.Publish(notificationEvent, cancellationToken);
            }
        }
        else if (request.NewStatus == OrderStatus.PickedUp)
        {
            var shipmentInfo = await dbContext.Shipments
                .Where(s => s.Id == order.ShipmentId)
                .Select(s => new { s.ShipmentStatus })
                .FirstOrDefaultAsync(cancellationToken);

            if (shipmentInfo != null && shipmentInfo.ShipmentStatus != ShipmentStatus.PickingUp && shipmentInfo.ShipmentStatus == ShipmentStatus.Assigned)
            {
                await dbContext.Shipments
                    .Where(s => s.Id == order.ShipmentId)
                    .ExecuteUpdateAsync(setters => setters
                        .SetProperty(s => s.ShipmentStatus, ShipmentStatus.PickingUp)
                        .SetProperty(s => s.TripStartedAt, DateTime.UtcNow),
                        cancellationToken);

                var customerUserId = await dbContext.Customers
                                    .Where(c => c.Id == order.CustomerId)
                                    .Select(c => c.UserId)
                                    .FirstOrDefaultAsync(cancellationToken);

                if (!string.IsNullOrEmpty(customerUserId))
                {

                    var notificationEvent = new ShipmentUpdateStatusEvent(
                        RecipientIds: new List<string> { customerUserId },
                        ShipmentId: order.ShipmentId.Value,
                        NewStatusName: "Picking Up"
                        );
                    await _mediator.Publish(notificationEvent, cancellationToken);
                }
            }
        }

        return Result<bool>.Success(true);
    }
}
