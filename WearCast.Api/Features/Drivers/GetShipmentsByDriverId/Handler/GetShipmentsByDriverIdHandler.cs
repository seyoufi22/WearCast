using WearCast.Api.Features.Drivers.GetDriverById.DTOs;
using WearCast.Api.Features.Drivers.GetShipmentsByDriverId.DTOs;

namespace WearCast.Api.Features.Drivers.GetShipmentsByDriverId.Handler
{
    public class GetShipmentsByDriverIdHandler : IRequestHandler<GetShipmentsByDriverIdRequestDTO, Result<IEnumerable<GetShipmentsByDriverIdResponse>>>
    {
        private readonly ApplicationDbContext _context;

        public GetShipmentsByDriverIdHandler(ApplicationDbContext context)
        {
            _context = context;
        }
        public async Task<Result<IEnumerable<GetShipmentsByDriverIdResponse>>> Handle(
            GetShipmentsByDriverIdRequestDTO request,
            CancellationToken cancellationToken)
        {
            var driverExists = await _context.Drivers.AnyAsync(d => d.Id == request.DriverId, cancellationToken);

            if (!driverExists)
            {
                return Result.Failure<IEnumerable<GetShipmentsByDriverIdResponse>>(DriverErrors.NotFound);
            }
            var shipments = await _context.Shipments
                .AsNoTracking()
                .Where(s => s.DriverId == request.DriverId)
                .OrderByDescending(s => s.CreatedOn)
                .Select(s => new GetShipmentsByDriverIdResponse
                {
                    Id = s.Id,
                    OrderTime = s.CreatedOn,
                    Price = s.Price,
                    ShipmentStatus = s.ShipmentStatus,
                    CustomerName = s.Customer.ApplicationUser.FirstName + " " + s.Customer.ApplicationUser.LastName,
                    CustomerPhoneNumber = s.Customer.ApplicationUser.PhoneNumber,
                    DeliveryCity = s.DeliveryAddress.City
                })
                .ToListAsync(cancellationToken);
            return Result.Success(shipments.AsEnumerable());
        }
    }
}
