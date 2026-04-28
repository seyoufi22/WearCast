using WearCast.Api.Abstractions;
using WearCast.Api.Features.Orders.GetOrdersByShipmentId.DTOs;
using WearCast.Api.Persistence;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace WearCast.Api.Features.Orders.GetOrdersByShipmentId.Query;

public class GetOrdersByShipmentIdQueryHandler(ApplicationDbContext dbContext)
    : IRequestHandler<GetOrdersByShipmentIdQuery, Result<GetOrdersByShipmentIdResponseDto>>
{
    public async Task<Result<GetOrdersByShipmentIdResponseDto>> Handle(
        GetOrdersByShipmentIdQuery request, CancellationToken cancellationToken)
    {
        var shipment = await dbContext.Shipments
            .AsNoTracking()
            .Include(s => s.Orders.Where(o => !o.IsDeleted))
            .Where(s => s.Id == request.ShipmentId && !s.IsDeleted)
            .FirstOrDefaultAsync(cancellationToken);

        if (shipment == null)
            return Result.Failure<GetOrdersByShipmentIdResponseDto>(
                new Error("Shipments.NotFound", $"Shipment with ID {request.ShipmentId} not found.",
                    Microsoft.AspNetCore.Http.StatusCodes.Status404NotFound));

        // Security check: only the customer who owns this shipment can view it
        if (shipment.CustomerId != request.CustomerId)
            return Result.Failure<GetOrdersByShipmentIdResponseDto>(
                new Error("Shipments.Forbidden", "You do not have permission to view this shipment.",
                    Microsoft.AspNetCore.Http.StatusCodes.Status403Forbidden));

        var response = new GetOrdersByShipmentIdResponseDto
        {
            ShipmentId = shipment.Id,
            ShipmentStatus = shipment.ShipmentStatus,
            DeliveryAddress = shipment.DeliveryAddress,
            Orders = shipment.Orders.Select(o => new ShipmentOrderDto
            {
                Id = o.Id,
                TotalAmount = o.TotalAmount,
                Status = o.Status,
                CreatedOn = o.CreatedOn,
                RecipientName = o.RecipientName,
                RecipientPhoneNumber = o.RecipientPhoneNumber,
                ShippingAddress = o.ShippingAddress,
                OrderType = o.SellerId.HasValue ? "Fixed" : "Designed"
            }).ToList()
        };

        return Result<GetOrdersByShipmentIdResponseDto>.Success(response);
    }
}
