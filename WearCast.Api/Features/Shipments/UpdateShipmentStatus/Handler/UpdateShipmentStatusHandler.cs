using System.Security.Claims;
using WearCast.Api.Features.Shipments.UpdateShipmentStatus.DTOs;

namespace WearCast.Api.Features.Shipments.UpdateShipmentStatus.Handler
{
    public class UpdateShipmentStatusHandler : IRequestHandler<UpdateShipmentStatusRequestDTO, Result>
    {
        private readonly ApplicationDbContext _context;

        public UpdateShipmentStatusHandler(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<Result> Handle(
            UpdateShipmentStatusRequestDTO request,
            CancellationToken cancellationToken)
        {
            var shipment = await _context.Shipments
                .Include(s => s.Driver)
                .FirstOrDefaultAsync(s => s.Id == request.ShipmentId, cancellationToken);

            if (shipment == null)
            {
                return Result.Failure(ShipmentErrors.NotFound);
            }

           

            bool isAdmin = true;
            bool isAssignedDriver = true;

            if (!isAdmin && !isAssignedDriver)
            {
                //un autorized
            }
           
            if (shipment.ShipmentStatus == ShipmentStatus.Delivered
                || shipment.ShipmentStatus == ShipmentStatus.UnAssigned)
            {
                return Result.Failure(ShipmentErrors.InvalidTransition);
            }

            if (shipment.ShipmentStatus == ShipmentStatus.Assigned)
            {
                if (request.NewStatus == ShipmentStatus.UnAssigned)
                {
                    shipment.DriverId = null;
                }
                else if (request.NewStatus != ShipmentStatus.OutForDelivery)
                {
                    return Result.Failure(ShipmentErrors.InvalidTransition);
                }
            }

            if (shipment.ShipmentStatus == ShipmentStatus.OutForDelivery &&
                 request.NewStatus != ShipmentStatus.Delivered)
            {
                return Result.Failure(ShipmentErrors.InvalidTransition);
            }


            shipment.ShipmentStatus = request.NewStatus;
            await _context.SaveChangesAsync(cancellationToken);

            return Result.Success();
        }
    }
}
