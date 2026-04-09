using WearCast.Api.Features.Drivers;
using WearCast.Api.Features.Shipments.Driver.AssignShipment.DTOs;

namespace WearCast.Api.Features.Shipments.Driver.AssignShipment.Handlers
{
    public class AssignShipmentHandler : IRequestHandler<AssignShipmentRequestDTO, Result>
    {
        private readonly ApplicationDbContext _context;

        public AssignShipmentHandler(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<Result> Handle(
            AssignShipmentRequestDTO request,
            CancellationToken cancellationToken)
        {

            var shipment = await _context.Shipments
                .FirstOrDefaultAsync(s => s.Id == request.ShipmentId, cancellationToken);

            if (shipment == null)
            {
                return Result.Failure(ShipmentErrors.NotFound);
            }

            if (shipment.ShipmentStatus != ShipmentStatus.Unassigned)
            {
                if (shipment.ShipmentStatus == ShipmentStatus.Pending)
                {
                    return Result.Failure(ShipmentErrors.NotReady);
                }
                else
                {
                    return Result.Failure(ShipmentErrors.AlreadyAssigned);
                }
            }
            var driver = await _context.Drivers
            .AsNoTracking()
            .FirstOrDefaultAsync(d => d.Id == request.DriverId, cancellationToken);

            if (driver == null)
            {
                return Result.Failure(DriverErrors.NotFound);
            }

            if (driver.Status != DriverStatus.Available)
            {
                return Result.Failure(DriverErrors.NotAvailable);
            }

            shipment.DriverId = request.DriverId;
            shipment.ShipmentStatus = ShipmentStatus.Assigned;
            shipment.UpdatedById = request.AssignerId;
            shipment.UpdatedOn=DateTime.UtcNow;
            await _context.SaveChangesAsync(cancellationToken);

            return Result.Success();
        }
    }
}
