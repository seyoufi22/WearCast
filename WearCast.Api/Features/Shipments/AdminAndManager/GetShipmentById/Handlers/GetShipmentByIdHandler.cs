using WearCast.Api.Features.Shipments.AdminAndManager.GetShipmentById.DTOs;

namespace WearCast.Api.Features.Shipments.AdminAndManager.GetShipmentById.Handlers
{
    public class GetShipmentByIdHandler : IRequestHandler<GetShipmentByIdRequestDTO, Result<GetShipmentByIdResponseDTO>>
    {
        private readonly ApplicationDbContext _context;

        public GetShipmentByIdHandler(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<Result<GetShipmentByIdResponseDTO>> Handle(
            GetShipmentByIdRequestDTO request,
            CancellationToken cancellationToken)
        {
            var shipment = await _context.Shipments
                .AsNoTracking()
                .Where(s => s.Id == request.ShipmentId)
                .Select(s => new GetShipmentByIdResponseDTO
                {
                    Id = s.Id,
                    DeliveryAddress = s.DeliveryAddress,
                    Price = s.Price,
                    ShipmentStatus = s.ShipmentStatus,
                    OrderTime = s.CreatedOn,
                    ReadyForPickupAt = s.ReadyForPickupAt,
                    TripStartedAt = s.TripStartedAt,
                    OutForDeliveryAt = s.OutForDeliveryAt,
                    DeliveredAt = s.DeliveredAt,
                    DeliveryCode = s.DeliveryCode,

                    DriverId = s.DriverId,
                    DriverName = s.Driver != null ?
                    s.Driver.ApplicationUser.FirstName + " " + s.Driver.ApplicationUser.LastName: null,
                    DriverPhoneNumber = s.Driver != null ?
                    s.Driver.ApplicationUser.PhoneNumber: null,
                    DriverNationalId = s.Driver != null ?s.Driver.NationalId: null,

                    CustomerId = s.CustomerId,
                    CustomerName = s.Customer.ApplicationUser.FirstName + " " + s.Customer.ApplicationUser.LastName,
                    CustomerPhoneNumber = s.Customer.ApplicationUser.PhoneNumber ?? string.Empty,
                })
                .FirstOrDefaultAsync(cancellationToken);
            if (shipment == null)
            {
                return Result.Failure<GetShipmentByIdResponseDTO>(ShipmentErrors.NotFound);
            }
            return Result.Success(shipment);
        }
    }
}
