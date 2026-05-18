using WearCast.Api.Features.Drivers.GetDriverById.DTOs;

namespace WearCast.Api.Features.Drivers.GetDriverById.Handlers
{
    public class GetDriverByIdHandler:IRequestHandler<GetDriverByIdRequestDTO, Result<GetDriverByIdResponseDTO>>
    {
        private readonly ApplicationDbContext _context;

        public GetDriverByIdHandler(ApplicationDbContext context)
        {
            _context = context;
        }
        public async Task<Result<GetDriverByIdResponseDTO>> Handle(
            GetDriverByIdRequestDTO request,
            CancellationToken cancellationToken)
        {         
            var driver = await _context.Drivers
                .AsNoTracking()
                .Where(d => d.Id == request.Id && d.ApplicationUser != null && d.ApplicationUser.IsDisabled == false)
                .Select(d => new GetDriverByIdResponseDTO
                {
                    Id = d.Id,
                    FirstName = d.ApplicationUser!.FirstName,
                    LastName = d.ApplicationUser.LastName,
                    Email = d.ApplicationUser.Email ?? string.Empty,
                    PhoneNumber = d.ApplicationUser.PhoneNumber ?? string.Empty,
                    ProfileImageUrl = d.ProfileImageUrl,
                    NationalId = d.NationalId,
                    VehicleType = d.VehicleType,
                    VehiclePlateNumber = d.VehiclePlateNumber,
                    Status = d.Status,
                    Address= d.Address,
                    NumberOfAssignedShipments=d.Shipments.Count(s=>s.ShipmentStatus==ShipmentStatus.Assigned),
                    NumberOfActiveShipments=d.Shipments.Count(s=>s.ShipmentStatus==ShipmentStatus.PickingUp|| s.ShipmentStatus == ShipmentStatus.OutForDelivery),
                    NumberOfDeliveredShipments =d.Shipments.Count(s=>s.ShipmentStatus==ShipmentStatus.Delivered)
                })
                .FirstOrDefaultAsync(cancellationToken);
            if (driver == null)
            {
                return Result.Failure<GetDriverByIdResponseDTO>(DriverErrors.NotFound);
            }
            return Result.Success(driver); 
        }
    }
}
