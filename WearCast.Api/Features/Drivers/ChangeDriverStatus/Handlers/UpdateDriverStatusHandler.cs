using WearCast.Api.Features.Drivers.ChangeDriverStatus.DTOs;
using WearCast.Api.Features.Shipments;

namespace WearCast.Api.Features.Drivers.ChangeDriverStatus.Handlers
{
    public class UpdateDriverStatusHandler : IRequestHandler<UpdateDriverStatusRequestDTO, Result>
    {
        private readonly ApplicationDbContext _context;
        private readonly IMediator _mediator;

        public UpdateDriverStatusHandler(ApplicationDbContext context, IMediator mediator)
        {
            _context = context;
            _mediator = mediator;
        }
        public async Task<Result> Handle(
            UpdateDriverStatusRequestDTO request,
            CancellationToken cancellationToken)
        {
            var driver = await _context.Drivers
                .Include(d => d.ApplicationUser)
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

            int affectedShipmentsCount = 0;

            if (request.NewStatus == DriverStatus.NotAvailable)
            {
                var hasActiveShipments = await _context.Shipments
                    .AnyAsync(s =>
                        s.DriverId == request.DriverId &&
                        (s.ShipmentStatus == ShipmentStatus.OutForDelivery ||
                        s.ShipmentStatus == ShipmentStatus.PickingUp),
                        cancellationToken);
                if (hasActiveShipments)
                {
                    return Result.Failure(DriverErrors.HasActiveShipments);
                }

                affectedShipmentsCount = await _context.Shipments
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
            if (affectedShipmentsCount>0)
            {
                var managersUserIds = await _context.ShippingCompanyManagers
                    .Where(m => m.ShippingCompanyId == driver.ShippingCompanyId && !m.IsDeleted)
                    .Select(m => m.UserId)
                    .ToListAsync(cancellationToken);

                if (managersUserIds.Any())
                {
                    var driverName = $"{driver.ApplicationUser?.FirstName} {driver.ApplicationUser?.LastName}".Trim();

                    var notificationEvent = new UpdateDriverStatusEvent(
                        RecipientIds: managersUserIds,
                        DriverName: string.IsNullOrEmpty(driverName) ? "Unknown Driver" : driverName,
                        AffectedShipmentsCount: affectedShipmentsCount
                    );

                    await _mediator.Publish(notificationEvent, cancellationToken);
                }
            }
            return Result.Success();
        }
    }
}

