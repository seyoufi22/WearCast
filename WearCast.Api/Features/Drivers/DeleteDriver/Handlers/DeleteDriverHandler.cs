using Microsoft.EntityFrameworkCore;
using WearCast.Api.Common.Enums;
using WearCast.Api.Features.Drivers.DeleteDriver.DTOs;

namespace WearCast.Api.Features.Drivers.DeleteDriver.Handlers
{
    public class DeleteDriverHandler : IRequestHandler<DeleteDriverRequestDTO, Result>
    {
        private readonly ApplicationDbContext _context;

        public DeleteDriverHandler(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<Result> Handle(DeleteDriverRequestDTO request, CancellationToken cancellationToken)
        {
            var driver = await _context.Drivers
                .Include(d => d.Shipments)
                .FirstOrDefaultAsync(d => d.Id == request.DriverId, cancellationToken);

            if (driver == null)
            {
                return Result.Failure(DriverErrors.NotFound);
            }

            // Check if manager is authorized (only for ShippingCompanyManager)
            var manager = await _context.ShippingCompanyManagers
                .FirstOrDefaultAsync(m => m.UserId == request.ManagerId, cancellationToken);

            if (manager != null)
            {
                if (driver.ShippingCompanyId != manager.ShippingCompanyId)
                {
                    return Result.Failure(DriverErrors.UnAuthorized);
                }
            }
            // SuperAdmin is also authorized but wouldn't be in ShippingCompanyManagers table typically

            // Check for active shipments
            bool hasActiveShipments = driver.Shipments.Any(s => s.ShipmentStatus != ShipmentStatus.Delivered);

            if (hasActiveShipments)
            {
                return Result.Failure(DriverErrors.HasActiveShipments);
            }

            driver.IsDeleted = true;
            await _context.SaveChangesAsync(cancellationToken);

            return Result.Success();
        }
    }
}
