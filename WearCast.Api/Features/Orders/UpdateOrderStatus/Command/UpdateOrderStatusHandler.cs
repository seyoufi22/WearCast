using WearCast.Api.Abstractions;
using WearCast.Api.Common.Enums;
using WearCast.Api.Features.Orders.UpdateOrderStatus.DTOs;
using WearCast.Api.Persistence;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace WearCast.Api.Features.Orders.UpdateOrderStatus.Command;

public class UpdateOrderStatusHandler(ApplicationDbContext dbContext)
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

        // Driver: can only mark Ready -> PickedUp
        if (request.DriverId.HasValue)
        {
            if (request.NewStatus != OrderStatus.PickedUp)
                return Result.Failure<bool>(new Error("Orders.InvalidTransition",
                    "Drivers can only mark orders as PickedUp.", StatusCodes.Status400BadRequest));

            if (order.Status != OrderStatus.Ready)
                return Result.Failure<bool>(new Error("Orders.InvalidTransition",
                    $"Cannot change status from {order.Status} to PickedUp. Order must be Ready first.", StatusCodes.Status400BadRequest));
        }

        order.Status = request.NewStatus;
        await dbContext.SaveChangesAsync(cancellationToken);

        return Result<bool>.Success(true);
    }
}
