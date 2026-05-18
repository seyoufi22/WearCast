using WearCast.Api.Features.Shipments.Driver.UnAssignShipment.DTOs;

namespace WearCast.Api.Features.Shipments.Driver.UnAssignShipment.Handlers
{
    public class UnAssignShipmentHandler : IRequestHandler<UnAssignShipmentRequestDTO, Result>
    {
        private readonly ApplicationDbContext _context;
        private readonly IMediator _mediator;
        public UnAssignShipmentHandler(ApplicationDbContext context, IMediator mediator)
        {
            _context = context;
            _mediator = mediator;
        }

        public async Task<Result> Handle(
            UnAssignShipmentRequestDTO request,
            CancellationToken cancellationToken)
        {
            var shipment = await _context.Shipments
                .AsNoTracking()
               .FirstOrDefaultAsync(s => s.Id == request.ShipmentId, cancellationToken);
            if (shipment == null)
            {
                return Result.Failure(ShipmentErrors.NotFound);
            }

            if (!request.IsAdmin)
            {
                var currentDriverId = await _context.Drivers
                    .AsNoTracking()
                    .Where(d => d.UserId == request.UpdaterId)
                    .Select(d => d.Id)
                    .FirstOrDefaultAsync(cancellationToken);
                if (currentDriverId == 0 || shipment.DriverId != currentDriverId)
                {
                    return Result.Failure(ShipmentErrors.UnAuthorized);
                }
            }
            if (shipment.ShipmentStatus != ShipmentStatus.Assigned)
            {
                return Result.Failure(ShipmentErrors.InvalidTransition);
            }

            var rowsAffected = await _context.Shipments
                .Where(s => s.Id == request.ShipmentId && s.ShipmentStatus == ShipmentStatus.Assigned)
                .ExecuteUpdateAsync(setters => setters
                    .SetProperty(s => s.DriverId, (int?)null)
                    .SetProperty(s => s.ShipmentStatus, ShipmentStatus.Unassigned)
                    .SetProperty(s => s.UpdatedById, request.UpdaterId)
                    .SetProperty(s => s.UpdatedOn, DateTime.UtcNow),
                cancellationToken);

            if (rowsAffected == 0)
            {
                return Result.Failure(ShipmentErrors.InvalidTransition);
            }

            if (!request.IsAdmin)
            {
                var managersUserIds = await _context.ShippingCompanyManagers
                  .Where(m => !m.IsDeleted)
                  .Select(m => m.UserId)
                  .ToListAsync(cancellationToken);
                if (managersUserIds.Any())
                {
                    var notificationEvent = new UnAssignShipmentEvent(
                    RecipientIds: managersUserIds,
                    ShipmentId: shipment.Id
                    );

                    await _mediator.Publish(notificationEvent, cancellationToken);
                }
            }
            return Result.Success();
        }
    }
}
