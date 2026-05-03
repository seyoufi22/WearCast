using Org.BouncyCastle.Cms;
using System.Security.Claims;
using WearCast.Api.Features.Shipments.Driver.UpdateShipmentStatus.DTOs;

namespace WearCast.Api.Features.Shipments.Driver.UpdateShipmentStatus.Handlers
{
    public class UpdateShipmentStatusHandler : IRequestHandler<UpdateShipmentStatusRequestDTO, Result>
    {
        private readonly ApplicationDbContext _context;
        private readonly IMediator _mediator;
        public UpdateShipmentStatusHandler(ApplicationDbContext context, IMediator mediator)
        {
            _context = context;
            _mediator = mediator;
        }

        public async Task<Result> Handle(
            UpdateShipmentStatusRequestDTO request,
            CancellationToken cancellationToken)
        {
            var shipment = await _context.Shipments
                .Include(s => s.Driver)
                .Include(s => s.Orders)
                .Include(s => s.Customer)
                .FirstOrDefaultAsync(s => s.Id == request.ShipmentId, cancellationToken);

            if (shipment == null)
            {
                return Result.Failure(ShipmentErrors.NotFound);
            }

            if (!request.IsAdmin)
            {
                if (shipment.Driver == null || shipment.Driver.UserId != request.UpdaterId)
                {
                    return Result.Failure(ShipmentErrors.UnAuthorized);
                }
            }

            if (shipment.ShipmentStatus == ShipmentStatus.Delivered
              || shipment.ShipmentStatus == ShipmentStatus.Unassigned
              || shipment.ShipmentStatus == ShipmentStatus.Pending)
            {
                return Result.Failure(ShipmentErrors.InvalidTransition);
            }

            if (shipment.ShipmentStatus == ShipmentStatus.Assigned)
            {
                if (request.NewStatus == ShipmentStatus.Unassigned)
                {
                    shipment.DriverId = null;
                }
                else if (request.NewStatus != ShipmentStatus.PickingUp)
                {
                    return Result.Failure(ShipmentErrors.InvalidTransition);
                }
                else
                {
                    bool Ready = shipment.Orders.All(o => o.Status == OrderStatus.Ready);
                    if (!Ready)
                    {
                        return Result.Failure(ShipmentErrors.NotReady);
                    }
                    shipment.TripStartedAt = DateTime.UtcNow;
                }
            }
            if (shipment.ShipmentStatus == ShipmentStatus.PickingUp)
            {
                if (request.NewStatus != ShipmentStatus.OutForDelivery)
                {
                    return Result.Failure(ShipmentErrors.InvalidTransition);
                }
                else
                {
                    bool PickedUp = shipment.Orders.All(o => o.Status == OrderStatus.PickedUp);
                    if (!PickedUp)
                    {
                        return Result.Failure(ShipmentErrors.NotPickedUp);
                    }
                    shipment.OutForDeliveryAt = DateTime.UtcNow;
                }

            }
            if (shipment.ShipmentStatus == ShipmentStatus.OutForDelivery)
            {
                if (request.NewStatus != ShipmentStatus.Delivered)
                {
                    return Result.Failure(ShipmentErrors.InvalidTransition);
                }
                if (string.IsNullOrWhiteSpace(request.DeliveryCode) || shipment.DeliveryCode != request.DeliveryCode)
                {
                    return Result.Failure(ShipmentErrors.WrongDeliveryCode);
                }
                shipment.DeliveredAt = DateTime.UtcNow;
            }

            shipment.UpdatedById = request.UpdaterId;
            shipment.UpdatedOn = DateTime.UtcNow;
            shipment.ShipmentStatus = request.NewStatus;

            await _context.SaveChangesAsync(cancellationToken);

            var recipients = new List<string> { shipment.Customer.UserId };

            var notificationEvent = new ShipmentUpdateStatusEvent(
                RecipientIds: recipients,
                ShipmentId: shipment.Id,
                NewStatusName: shipment.ShipmentStatus.ToString()
            );

            await _mediator.Publish(notificationEvent, cancellationToken);

            return Result.Success();
        }
    }
}
