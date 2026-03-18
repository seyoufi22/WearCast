using System.Security.Claims;
using WearCast.Api.Features.Drivers.GetAllDrivers.DTOs;

namespace WearCast.Api.Features.Drivers.GetAllDrivers.Handlers
{
    public class GetAllDriversHandler : IRequestHandler<GetAllDriversRequestDTO, Result<IEnumerable<GetAllDriversResponseDTO>>>
    {
        private readonly ApplicationDbContext _context;

        public GetAllDriversHandler(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<Result<IEnumerable<GetAllDriversResponseDTO>>> Handle(
            GetAllDriversRequestDTO request,
            CancellationToken cancellationToken)
        {
            var drivers = await _context.Drivers
                .AsNoTracking()
                .Where(d => d.ApplicationUser != null && d.ApplicationUser.IsDisabled == false)
                .Select(d => new GetAllDriversResponseDTO
                {
                    Id = d.Id,
                    DriverName = d.ApplicationUser!.FirstName + " " + d.ApplicationUser.LastName,
                    VehicleType = d.VehicleType,
                    ShipmentsCount = d.Shipments.Count(s =>
                    s.ShipmentStatus == ShipmentStatus.Assigned||
                    s.ShipmentStatus==ShipmentStatus.OutForDelivery),
                    Status = d.Status
                })
                .ToListAsync(cancellationToken);
            return Result.Success(drivers.AsEnumerable());
        }
    }
}