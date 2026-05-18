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

            if (shipment.ShipmentStatus == ShipmentStatus.PickingUp)
            {
                if (request.NewStatus != ShipmentStatus.OutForDelivery)
                {
                    return Result.Failure(ShipmentErrors.InvalidTransition);
                }
                else
                {
                    bool hasUnpickedOrders = await _context.Orders
                        .AnyAsync(o => o.ShipmentId == shipment.Id && o.Status != OrderStatus.PickedUp, cancellationToken);

                    if (hasUnpickedOrders)
                    {
                        return Result.Failure(ShipmentErrors.NotPickedUp);
                    }

                    shipment.OutForDeliveryAt = DateTime.UtcNow;
                }
            }
            else if (shipment.ShipmentStatus == ShipmentStatus.OutForDelivery)
            {
                if (request.NewStatus != ShipmentStatus.Delivered)
                {
                    return Result.Failure(ShipmentErrors.InvalidTransition);
                }
                if (string.IsNullOrWhiteSpace(request.DeliveryCode) ||
                    !string.Equals(shipment.DeliveryCode, request.DeliveryCode))
                {
                    return Result.Failure(ShipmentErrors.WrongDeliveryCode);
                }
                shipment.DeliveredAt = DateTime.UtcNow;
            }
            else
            {
                return Result.Failure(ShipmentErrors.InvalidTransition);
            }

            shipment.UpdatedById = request.UpdaterId;
            shipment.UpdatedOn = DateTime.UtcNow;
            shipment.ShipmentStatus = request.NewStatus;

            await _context.SaveChangesAsync(cancellationToken);

            var customerUserId = await _context.Customers
                .Where(c => c.Id == shipment.CustomerId) 
                .Select(c => c.UserId)
                .FirstOrDefaultAsync(cancellationToken);

            var recipients = new List<string> { customerUserId };

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
