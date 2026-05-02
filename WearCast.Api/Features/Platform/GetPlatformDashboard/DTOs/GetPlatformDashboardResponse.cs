namespace WearCast.Api.Features.Platform.GetPlatformDashboard.DTOs;

public record GetPlatformDashboardResponse(
    decimal TotalMoneyProcessed,
    decimal PlatformRevenue,
    decimal PlatformWalletBalance,
    int TotalShipments,
    int TotalOrderItems,
    int TotalProducts,
    int TotalSellers,
    int TotalCustomers,
    int TotalSellerOrders,
    int TotalFactoryOrders,
    decimal CommissionPercentage
);
