namespace WearCast.Api.Features.Sellers.GetDashboardStats;

public record SellerDashboardStatsResponse(
    decimal TotalRevenue,
    int TotalOrders,
    int PendingOrders,
    int UniqueProductsCount,
    int TotalInventoryItems
);