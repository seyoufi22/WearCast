using WearCast.Api.Features.ShippingCompanies.Dashboard.DTOs;

namespace WearCast.Api.Features.ShippingCompanies.Dashboard.Handlers
{
    public class ShippingCompanyDashboardHandler(ApplicationDbContext dbContext)
        : IRequestHandler<ShippingCompanyDashboardRequestDTO, Result<ShippingCompanyDashboardResponseDTO>>
    {
        public async Task<Result<ShippingCompanyDashboardResponseDTO>> Handle(
            ShippingCompanyDashboardRequestDTO request,
            CancellationToken cancellationToken)
        {
            var orderStats = await dbContext.Orders
                .AsNoTracking()
                .GroupBy(o => o.Status)
                .Select(g => new { Status = g.Key, Count = g.Count() })
                .ToDictionaryAsync(k => k.Status, v => v.Count, cancellationToken);

            var shipmentStats = await dbContext.Shipments
                .AsNoTracking()
                .GroupBy(s => s.ShipmentStatus)
                .Select(g => new { Status = g.Key, Count = g.Count() })
                .ToDictionaryAsync(k => k.Status, v => v.Count, cancellationToken);

            var driverStats = await dbContext.Drivers
                .AsNoTracking()
                .GroupBy(d => d.Status)
                .Select(g => new { Status = g.Key, Count = g.Count() })
                .ToDictionaryAsync(k => k.Status, v => v.Count, cancellationToken);

            var avgDeliveryInMinutes = await dbContext.Shipments
                .AsNoTracking()
                .Where(s => s.ReadyForPickupAt.HasValue && s.DeliveredAt.HasValue)
                .Select(s => (double?)EF.Functions.DateDiffMinute(s.ReadyForPickupAt, s.DeliveredAt))
                .AverageAsync(cancellationToken) ?? 0;

            double averageDeliveryHours = Math.Round(avgDeliveryInMinutes / 60.0, 1);

            var totalManagers = await dbContext.ShippingCompanyManagers
                .Where(m => !m.IsDeleted)
                .CountAsync(cancellationToken);

            var response = new ShippingCompanyDashboardResponseDTO
            {
                PendingOrders = orderStats.GetValueOrDefault(OrderStatus.Ready),
                PickedUpOrders = orderStats.GetValueOrDefault(OrderStatus.PickedUp),

                PendingShipments = shipmentStats.GetValueOrDefault(ShipmentStatus.Pending),
                UnassignedShipments = shipmentStats.GetValueOrDefault(ShipmentStatus.Unassigned),
                AssignedShipments = shipmentStats.GetValueOrDefault(ShipmentStatus.Assigned),
                PickingUpShipments = shipmentStats.GetValueOrDefault(ShipmentStatus.PickingUp),
                OutForDeliveryShipments = shipmentStats.GetValueOrDefault(ShipmentStatus.OutForDelivery),
                DeliveredShipments = shipmentStats.GetValueOrDefault(ShipmentStatus.Delivered),

                TotalDrivers = driverStats.Values.Sum(),
                ActiveDrivers = driverStats.GetValueOrDefault(DriverStatus.Available),
                InactiveDrivers = driverStats.GetValueOrDefault(DriverStatus.NotAvailable),
                AverageDeliveryTimeInHours = averageDeliveryHours,
                NumberOfManagers=totalManagers
            };

            return Result.Success(response);
        }
    }
}
