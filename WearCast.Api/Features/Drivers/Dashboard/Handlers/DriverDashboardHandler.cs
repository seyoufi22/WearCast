using WearCast.Api.Features.Drivers.Dashboard.DTOs;

namespace WearCast.Api.Features.Drivers.Dashboard.Handlers
{
    public class DriverDashboardHandler : IRequestHandler<DriverDashboardRequestDTO, Result<DriverDashboardResponseDTO>>
    {
        private readonly ApplicationDbContext _context;

        public DriverDashboardHandler(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<Result<DriverDashboardResponseDTO>> Handle(DriverDashboardRequestDTO request, CancellationToken cancellationToken)
        {
            var driverInfo = await _context.Drivers
                .AsNoTracking()
                .Where(d => d.Id == request.DriverId)
                .Select(d => new
                {
                    Status = d.Status,
                    VehicleType = d.VehicleType
                })
                .FirstOrDefaultAsync(cancellationToken);

            if (driverInfo == null)
            {
                return Result.Failure<DriverDashboardResponseDTO>(DriverErrors.NotFound);
            }

            var shipmentStats = await _context.Shipments
                .AsNoTracking()
                .Where(s => s.DriverId == request.DriverId)
                .GroupBy(s => s.ShipmentStatus)
                .Select(g => new { Status = g.Key, Count = g.Count() })
                .ToDictionaryAsync(k => k.Status, v => v.Count, cancellationToken);

            var orderStats = await _context.Orders
                .AsNoTracking()
                .Where(o => o.Shipment != null && o.Shipment.DriverId == request.DriverId)
                .GroupBy(o => o.Status)
                .Select(g => new { Status = g.Key, Count = g.Count() })
                .ToDictionaryAsync(k => k.Status, v => v.Count, cancellationToken);

            var response = new DriverDashboardResponseDTO
            {
                DriverStatus = driverInfo.Status,
                DeliveryVehicleType = driverInfo.VehicleType,

                PendingOrders = orderStats.GetValueOrDefault(OrderStatus.Ready),
                PickedUpOrders = orderStats.GetValueOrDefault(OrderStatus.PickedUp),

                AssignedShipments = shipmentStats.GetValueOrDefault(ShipmentStatus.Assigned),
                PickingUpShipments = shipmentStats.GetValueOrDefault(ShipmentStatus.PickingUp),
                OutForDeliveryShipments = shipmentStats.GetValueOrDefault(ShipmentStatus.OutForDelivery),
                DeliveredShipments = shipmentStats.GetValueOrDefault(ShipmentStatus.Delivered)
            };

            return Result.Success(response);
        }
    }
}