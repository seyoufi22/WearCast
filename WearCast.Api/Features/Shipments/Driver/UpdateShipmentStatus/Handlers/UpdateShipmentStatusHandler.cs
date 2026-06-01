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
            var shipmentInfo = await _context.Shipments
                .AsNoTracking()
                .Where(s => s.Id == request.ShipmentId)
                .Select(s => new
                {
                    s.Id,
                    s.ShipmentStatus,
                    s.DriverId,
                    s.DeliveryCode,
                    CustomerUserId = s.Customer.UserId
                })
                .FirstOrDefaultAsync(cancellationToken);

            if (shipmentInfo == null)
            {
                return Result.Failure(ShipmentErrors.NotFound);
            }

            if (!request.IsAdmin)
            {
                var currentDriverId = await _context.Drivers
                     .Where(d => d.UserId == request.UpdaterId)
                     .Select(d => d.Id)
                     .FirstOrDefaultAsync(cancellationToken);
                if (currentDriverId == 0 || shipmentInfo.DriverId != currentDriverId)
                {
                    return Result.Failure(ShipmentErrors.UnAuthorized);
                }
            }

            int rowsAffected = 0;

            if (shipmentInfo.ShipmentStatus == ShipmentStatus.PickingUp)
            {
                if (request.NewStatus != ShipmentStatus.OutForDelivery)
                {
                    return Result.Failure(ShipmentErrors.InvalidTransition);
                }

                bool hasUnpickedOrders = await _context.Orders
                    .AnyAsync(o => o.ShipmentId == shipmentInfo.Id && o.Status != OrderStatus.PickedUp, cancellationToken);

                if (hasUnpickedOrders)
                {
                    return Result.Failure(ShipmentErrors.NotPickedUp);
                }

                rowsAffected = await _context.Shipments
                    .Where(s => s.Id == request.ShipmentId && s.ShipmentStatus == ShipmentStatus.PickingUp)
                    .ExecuteUpdateAsync(setters => setters
                    .SetProperty(s => s.ShipmentStatus, request.NewStatus)
                    .SetProperty(s => s.OutForDeliveryAt, DateTime.UtcNow)
                    .SetProperty(s => s.UpdatedById, request.UpdaterId)
                    .SetProperty(s => s.UpdatedOn, DateTime.UtcNow),
                    cancellationToken);
            }
            else if (shipmentInfo.ShipmentStatus == ShipmentStatus.OutForDelivery)
            {
                if (request.NewStatus != ShipmentStatus.Delivered)
                {
                    return Result.Failure(ShipmentErrors.InvalidTransition);
                }
                if (string.IsNullOrWhiteSpace(request.DeliveryCode) ||
                    !string.Equals(shipmentInfo.DeliveryCode, request.DeliveryCode))
                {
                    return Result.Failure(ShipmentErrors.WrongDeliveryCode);
                }
                rowsAffected = await _context.Shipments
                    .Where(s => s.Id == request.ShipmentId && s.ShipmentStatus == ShipmentStatus.OutForDelivery)
                    .ExecuteUpdateAsync(setters => setters
                    .SetProperty(s => s.ShipmentStatus, request.NewStatus)
                    .SetProperty(s => s.DeliveredAt, DateTime.UtcNow)
                    .SetProperty(s => s.UpdatedById, request.UpdaterId)
                    .SetProperty(s => s.UpdatedOn, DateTime.UtcNow),
                    cancellationToken);
            }
            else
            {
                return Result.Failure(ShipmentErrors.InvalidTransition);
            }

            if (rowsAffected == 0)
            {
                return Result.Failure(ShipmentErrors.InvalidTransition);
            }


            if (!string.IsNullOrEmpty(shipmentInfo.CustomerUserId))
            {
                var notificationEvent = new ShipmentUpdateStatusEvent(
                    RecipientIds: new List<string> { shipmentInfo.CustomerUserId },
                    ShipmentId: shipmentInfo.Id,
                    NewStatusName: request.NewStatus.ToString()
                );

                await _mediator.Publish(notificationEvent, cancellationToken);
            }
            return Result.Success();
        }
    }
}
