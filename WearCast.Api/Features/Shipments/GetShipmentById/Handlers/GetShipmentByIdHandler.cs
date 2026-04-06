using WearCast.Api.Features.Drivers;
using WearCast.Api.Features.Drivers.GetDriverById.DTOs;
using WearCast.Api.Features.Shipments.GetShipmentById.DTOs;

namespace WearCast.Api.Features.Shipments.GetShipmentById.Handlers
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
                .Where(s => s.Id == request.Id)
                .Select(s => new GetShipmentByIdResponseDTO
                {
                    Id = s.Id,
                    PickUpAddress = s.PickUpAddress,
                    DeliveryAddress = s.DeliveryAddress,
                    OrderTime = s.CreatedOn,
                    Price = s.Price,
                    ShipmentStatus = s.ShipmentStatus,
                    CustomerID = s.CustomerID,
                    CustomerName = s.Customer.ApplicationUser.FirstName + " " + s.Customer.ApplicationUser.LastName,
                    CustomerPhoneNumber = s.Customer.ApplicationUser.PhoneNumber,
                    DriverId = s.DriverId,
                    DriverName = s.Driver != null ? s.Driver.ApplicationUser.FirstName + " " + s.Driver.ApplicationUser.LastName : null,
                    DriverPhoneNumber = s.Driver != null ? s.Driver.ApplicationUser.PhoneNumber : null
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
