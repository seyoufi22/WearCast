using WearCast.Api.Features.Shipments.Customer.GetShipmentById.DTOs;

namespace WearCast.Api.Features.Shipments.Customer.GetShipmentById.Handlers
{
    public class GetCustomerShipmentByIdHandler : IRequestHandler<GetCustomerShipmentByIdRequestDTO, Result<GetCustomerShipmentByIdResponseDTO>>
    {
        private readonly ApplicationDbContext _context;

        public GetCustomerShipmentByIdHandler(ApplicationDbContext context)
        {
            _context = context;
        }
        public async Task<Result<GetCustomerShipmentByIdResponseDTO>> Handle(
            GetCustomerShipmentByIdRequestDTO request,
            CancellationToken cancellationToken)
        {
            var shipment = await _context.Shipments
                .AsNoTracking()
                .Where(s =>
                s.Id == request.ShipmentId && s.CustomerId == request.CustomerId)
                .Select(s => new GetCustomerShipmentByIdResponseDTO
                {
                    Id = s.Id,
                    Price = s.Price,
                    ShipmentStatus = s.ShipmentStatus,
                    OrderedAt = s.CreatedOn,
                    ReadyForPickupAt = s.ReadyForPickupAt,
                    TripStartedAt = s.TripStartedAt,
                    OutForDeliveryAt = s.OutForDeliveryAt,
                    DeliveredAt = s.DeliveredAt,

                    DriverName = s.Driver != null ?
                    s.Driver.ApplicationUser.FirstName + " " + s.Driver.ApplicationUser.LastName
                    : null,
                    DriverPhoneNumber = s.Driver != null ?
                    s.Driver.ApplicationUser.PhoneNumber
                    : null,

                    DeliveryAddress = s.DeliveryAddress,
                    DeliveryCode = s.DeliveryCode,
                })
                .FirstOrDefaultAsync(cancellationToken);
            if (shipment == null)
            {
                return Result.Failure<GetCustomerShipmentByIdResponseDTO>(ShipmentErrors.NotFound);
            }
            return Result.Success(shipment);
        }
    }
}
