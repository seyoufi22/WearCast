namespace WearCast.Api.Features.Shipments.AdminAndManager.GetShippingStats.DTOs
{
    public record ShippingStatsResponse(
        int TotalShipments,
        int ActiveDrivers,
        decimal TotalRevenue,
        int PendingDeliveries,
        double TotalShipmentsGrowth,
        double ActiveDriversGrowth,
        double TotalRevenueGrowth,
        double PendingDeliveriesGrowth,
        List<MonthlyRevenueDto> MonthlyRevenue,
        Dictionary<string, int> StatusBreakdown
    );

    public record MonthlyRevenueDto(string Month, decimal Revenue);
}
