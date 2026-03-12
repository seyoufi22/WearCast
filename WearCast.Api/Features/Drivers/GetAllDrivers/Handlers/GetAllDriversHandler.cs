using Microsoft.EntityFrameworkCore;
using WearCast.Api.Features.Drivers.GetAllDrivers.DTOs;
using MediatR;

namespace WearCast.Api.Features.Drivers.GetAllDrivers.Handlers
{
    public class GetAllDriversHandler : IRequestHandler<GetAllDriversRequestDTO, IEnumerable<GetAllDriversResponseDTO>>
    {
        private readonly ApplicationDbContext _context;

        public GetAllDriversHandler(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<GetAllDriversResponseDTO>> Handle(
            GetAllDriversRequestDTO request,
            CancellationToken cancellationToken)
        {
            var drivers = await _context.Drivers
                .Where(d => d.ApplicationUser != null && d.ApplicationUser.IsDisabled == false)
                .Select(d => new GetAllDriversResponseDTO
                {
                    Id = d.Id,
                    DriverName = d.ApplicationUser!.FirstName + " " + d.ApplicationUser.LastName,
                    VehicleType = d.VehicleType,
                    VehiclePlateNumber = d.VehiclePlateNumber,
                    ShipmentsCount = d.Shipments.Count(),
                    Status = d.Status
                })
                .ToListAsync(cancellationToken); 
            return drivers;
        }
    }
}