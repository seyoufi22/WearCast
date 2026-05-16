namespace WearCast.Api.Features.Sellers.GetDashboardStats;

public record SellerDashboardStatsResponse(
    decimal TotalRevenue,
    int TotalOrders,
    int PendingOrders,
    int UniqueProductsCount,
    int TotalInventoryItems,
    IEnumerable<TopSellingProductDto> TopSellingProducts
);

public record TopSellingProductDto(
    int ProductId,
    string Name,
    TargetAudience TargetAudience,
    decimal Price,
    int Stock,
    bool IsRejected,
    string? MainImageUrl,
    int TotalSold
);