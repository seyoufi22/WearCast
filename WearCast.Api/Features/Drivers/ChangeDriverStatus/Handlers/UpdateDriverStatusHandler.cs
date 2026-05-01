using WearCast.Api.Features.Drivers.ChangeDriverStatus.DTOs;
using WearCast.Api.Features.Shipments;

namespace WearCast.Api.Features.Drivers.ChangeDriverStatus.Handlers
{
    public class UpdateDriverStatusHandler : IRequestHandler<UpdateDriverStatusRequestDTO, Result>
    {
        private readonly ApplicationDbContext _context;

        public UpdateDriverStatusHandler(ApplicationDbContext context)
        {
            _context = context;
        }
        public async Task<Result> Handle(
            UpdateDriverStatusRequestDTO request,
            CancellationToken cancellationToken)
        {
            var driver = await _context.Drivers
                .FirstOrDefaultAsync(d => d.Id == request.DriverId, cancellationToken);
            if (driver == null)
            {
                return Result.Failure(DriverErrors.NotFound);
            }

            if (!request.IsAdmin)
            {
                if (driver.UserId != request.UpdaterId)
                {
                    return Result.Failure(DriverErrors.UnAuthorized);
                }
            }

            if (driver.Status == request.NewStatus)
            {
                return Result.Success();
            }
            if (request.NewStatus == DriverStatus.NotAvailable)
            {
                await _context.Shipments
                    .Where(s => s.DriverId == request.DriverId &&
                                s.ShipmentStatus == ShipmentStatus.Assigned)
                    .ExecuteUpdateAsync(setters => setters
                        .SetProperty(s => s.ShipmentStatus, ShipmentStatus.Unassigned)
                        .SetProperty(s => s.DriverId, (int?)null)
                        .SetProperty(s => s.UpdatedById, request.UpdaterId)
                        .SetProperty(s => s.UpdatedOn, DateTime.UtcNow),
                        cancellationToken);
            }
            driver.Status = request.NewStatus;

            await _context.SaveChangesAsync(cancellationToken);

            return Result.Success();
        }
    }
}

