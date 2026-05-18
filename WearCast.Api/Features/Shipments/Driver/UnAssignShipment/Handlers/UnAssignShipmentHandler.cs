using WearCast.Api.Features.Shipments.Driver.UnAssignShipment.DTOs;
using WearCast.Api.Features.Shipments.Driver.UpdateShipmentStatus;
using WearCast.Api.Features.Shipments.Driver.UpdateShipmentStatus.DTOs;

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
               .FirstOrDefaultAsync(s => s.Id == request.ShipmentId, cancellationToken);
            if (shipment == null)
            {
                return Result.Failure(ShipmentErrors.NotFound);
            }

            if (!request.IsAdmin)
            {
                var currentDriverId = await _context.Drivers
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
            shipment.DriverId = null;
            shipment.UpdatedById = request.UpdaterId;
            shipment.UpdatedOn = DateTime.UtcNow;
            shipment.ShipmentStatus = ShipmentStatus.Unassigned;

            await _context.SaveChangesAsync(cancellationToken);

            if (!request.IsAdmin)
            {
                var managersUserIds = await _context.ShippingCompanyManagers
                  .Where(m => !m.IsDeleted)
                  .Select(m => m.UserId)
                  .ToListAsync(cancellationToken);

                var notificationEvent = new UnAssignShipmentEvent(
                    RecipientIds: managersUserIds,
                    ShipmentId: shipment.Id
                );

                await _mediator.Publish(notificationEvent, cancellationToken);
            }
            return Result.Success();
        }
    }
}
