using WearCast.Api.Features.Drivers.DeleteDriver.DTOs;

namespace WearCast.Api.Features.Drivers.DeleteDriver.Handlers
{
    public class DeleteDriverHandler : IRequestHandler<DeleteDriverRequestDTO, Result>
    {
        private readonly ApplicationDbContext _context;
        private readonly IMediator _mediator;

        public DeleteDriverHandler(ApplicationDbContext context, IMediator mediator)
        {
            _context = context;
            _mediator = mediator;
        }

        public async Task<Result> Handle(
            DeleteDriverRequestDTO request,
            CancellationToken cancellationToken)
        {
            var driver = await _context.Drivers
                .Include(d => d.ApplicationUser)
                .FirstOrDefaultAsync(d => d.Id == request.DriverId && !d.IsDeleted, cancellationToken);

            if (driver == null)
            {
                return Result.Failure(DriverErrors.NotFound);
            }

            var hasActiveShipments = await _context.Shipments
                .AnyAsync(s =>
                    s.DriverId == request.DriverId &&
                    (s.ShipmentStatus == ShipmentStatus.OutForDelivery ||
                    s.ShipmentStatus == ShipmentStatus.PickingUp),
                    cancellationToken);

            if (hasActiveShipments)
            {
                return Result.Failure(DriverErrors.DeleteActiveDriver);
            }

            int affectedShipmentsCount = await _context.Shipments
                .Where(s => s.DriverId == request.DriverId &&
                            s.ShipmentStatus == ShipmentStatus.Assigned)
                .ExecuteUpdateAsync(setters => setters
                    .SetProperty(s => s.ShipmentStatus, ShipmentStatus.Unassigned)
                    .SetProperty(s => s.DriverId, (int?)null)
                    .SetProperty(s => s.UpdatedById, request.UpdaterId)
                    .SetProperty(s => s.UpdatedOn, DateTime.UtcNow),
                    cancellationToken);

            driver.IsDeleted = true;

            if (driver.ApplicationUser != null)
            {
                driver.ApplicationUser.IsDeleted = true;
            }

            await _context.SaveChangesAsync(cancellationToken);

            if (affectedShipmentsCount > 0)
            {
                var managersUserIds = await _context.ShippingCompanyManagers
                    .Where(m => m.ShippingCompanyId == driver.ShippingCompanyId && !m.IsDeleted)
                    .Select(m => m.UserId)
                    .ToListAsync(cancellationToken);

                if (managersUserIds.Any())
                {
                    var driverName = $"{driver.ApplicationUser?.FirstName} {driver.ApplicationUser?.LastName}".Trim();

                    var notificationEvent = new DeleteDriverEvent(
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
