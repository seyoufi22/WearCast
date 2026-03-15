using Microsoft.AspNetCore.Http.HttpResults;
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
                })
                .FirstOrDefaultAsync(cancellationToken);
            if (driver == null)
            {
                return Result.Failure<GetDriverByIdResponseDTO>(new Error(
                    "Driver.NotFound",
                    $"No active driver found with ID {request.Id}",
                    StatusCodes.Status404NotFound));
            }
            return Result.Success(driver); 
        }
    }
}
