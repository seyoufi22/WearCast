using WearCast.Api.Features.Drivers.GetAllDrivers.DTOs;

namespace WearCast.Api.Features.Drivers.GetAllDrivers.Handlers
{
    public class GetAllDriversHandler : IRequestHandler<GetAllDriversRequestDTO, IEnumerable<GetAllDriversResponseDTO>>
    {
        private readonly ApplicationDbContext context;

        public GetAllDriversHandler(ApplicationDbContext _context)
        {
            context = _context;
        }
        public async Task<IEnumerable<GetAllDriversResponseDTO>> Handle(
        GetAllDriversRequestDTO request,
        CancellationToken cancellationToken)
        {   
            var drivers = await context.Drivers
                .Select(d => new GetAllDriversResponseDTO
                {
                     Id = d.Id,
                     DriverName = (d.ApplicationUser != null ? d.ApplicationUser.FirstName + " " + d.ApplicationUser.LastName : ""),
                     VehicleType = d.VehicleType,
                     VehiclePlateNumber = d.VehiclePlateNumber,
                     ShipmentsCount = d.Shipments.Count(),
                     Status = d.Status
                }).ToListAsync();
            return drivers;
        }
    }
}
