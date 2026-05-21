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
            .Include(s => s.Driver)
                .ThenInclude(d => d!.ApplicationUser)
            .Include(s => s.Customer)
                .ThenInclude(c => c!.ApplicationUser)
            .Include(s => s.Orders.Where(o => !o.IsDeleted))
                .ThenInclude(o => o.Seller)
            .Include(s => s.Orders.Where(o => !o.IsDeleted))
                .ThenInclude(o => o.Factory)
            .Where(s => s.Id == request.ShipmentId && !s.IsDeleted)
            .FirstOrDefaultAsync(cancellationToken);

        if (shipment == null)
            return Result.Failure<GetOrdersByShipmentIdResponseDto>(
                new Error("Shipments.NotFound", $"Shipment with ID {request.ShipmentId} not found.",
                    Microsoft.AspNetCore.Http.StatusCodes.Status404NotFound));

        // Security check: admins, the customer who owns the shipment, or the assigned driver can view it
        var isAssignedDriver = request.DriverId.HasValue && shipment.DriverId == request.DriverId;
        if (!request.IsAdmin && shipment.CustomerId != request.CustomerId && !isAssignedDriver)
            return Result.Failure<GetOrdersByShipmentIdResponseDto>(
                new Error("Shipments.Forbidden", "You do not have permission to view this shipment.",
                    Microsoft.AspNetCore.Http.StatusCodes.Status403Forbidden));

        var response = new GetOrdersByShipmentIdResponseDto
        {
            Id = shipment.Id,
            IsDeleted = shipment.IsDeleted,
            DeliveryAddress = shipment.DeliveryAddress,
            Price = shipment.Price,
            ShipmentStatus = shipment.ShipmentStatus,
            OrderTime = shipment.CreatedOn,
            ReadyForPickupAt = shipment.ReadyForPickupAt,
            TripStartedAt = shipment.TripStartedAt,
            OutForDeliveryAt = shipment.OutForDeliveryAt,
            DeliveredAt = shipment.DeliveredAt,
            DeliveryCode = shipment.DeliveryCode,
            DriverId = shipment.DriverId,
            DriverName = shipment.Driver?.ApplicationUser != null ? $"{shipment.Driver.ApplicationUser.FirstName} {shipment.Driver.ApplicationUser.LastName}" : null,
            DriverPhoneNumber = shipment.Driver?.ApplicationUser?.PhoneNumber,
            DriverNationalId = shipment.Driver?.NationalId,
            CustomerId = shipment.CustomerId,
            CustomerName = shipment.Customer?.ApplicationUser != null ? $"{shipment.Customer.ApplicationUser.FirstName} {shipment.Customer.ApplicationUser.LastName}" : string.Empty,
            CustomerPhoneNumber = shipment.Customer?.ApplicationUser?.PhoneNumber ?? string.Empty,
            Orders = shipment.Orders.Select(o => new ShipmentOrderDto
            {
                Id = o.Id,
                TotalAmount = o.TotalAmount,
                Commission = o.Commission,
                Payout = o.Payout,
                Status = o.Status,
                CreatedOn = o.CreatedOn,
                RecipientName = o.RecipientName,
                RecipientPhoneNumber = o.RecipientPhoneNumber,
                ShippingAddress = o.ShippingAddress,
                OrderType = o.SellerId.HasValue ? "Fixed" : "Designed",
                VendorName = o.Seller != null ? o.Seller.Name : (o.Factory != null ? o.Factory.Name : string.Empty),
                VendorPhoneNumber = o.Seller != null ? o.Seller.PhoneNumber : (o.Factory != null ? o.Factory.PhoneNumber : string.Empty)
            }).ToList()
        };

        return Result<GetOrdersByShipmentIdResponseDto>.Success(response);
    }
}
