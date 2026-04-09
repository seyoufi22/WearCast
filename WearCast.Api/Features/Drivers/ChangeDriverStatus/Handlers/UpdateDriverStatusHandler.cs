using WearCast.Api.Features.Drivers.ChangeDriverStatus.DTOs;

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
            if (driver.Status == request.NewStatus)
            {
                return Result.Success();
            }
            if (request.NewStatus == DriverStatus.NotAvailable)
            {
                var hasOutForDelivery = await _context.Shipments
                    .AnyAsync(s =>
                        s.DriverId == request.DriverId &&
                        s.ShipmentStatus == ShipmentStatus.OutForDelivery,
                        cancellationToken);
                if (hasOutForDelivery)
                {
                    return Result.Failure(DriverErrors.HasOutForDeliveryShipments);
                }

                var assignedShipments = await _context.Shipments
                    .Where(s =>
                        s.DriverId == request.DriverId &&
                        s.ShipmentStatus == ShipmentStatus.Assigned)
                    .ToListAsync(cancellationToken);

                foreach (var shipment in assignedShipments)
                {
                    shipment.ShipmentStatus = ShipmentStatus.Unassigned;
                    shipment.DriverId = null;
                }
                /*
                 * another imp
                 * 
                 await _context.Shipments
            .Where(s =>
                s.DriverId == request.DriverId &&
                s.ShipmentStatus == ShipmentStatus.Assigned)
            .ExecuteUpdateAsync(s => s
                .SetProperty(x => x.DriverId, (int?)null)
                .SetProperty(x => x.ShipmentStatus, ShipmentStatus.Unassigned),
                cancellationToken);
                */
            }

            driver.Status = request.NewStatus;

            await _context.SaveChangesAsync(cancellationToken);

            return Result.Success();
        }
    }
}

