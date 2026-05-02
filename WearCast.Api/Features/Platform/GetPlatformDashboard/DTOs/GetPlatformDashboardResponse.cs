namespace WearCast.Api.Features.Platform.GetPlatformDashboard.DTOs;

public record GetPlatformDashboardResponse(
    decimal TotalMoneyProcessed,
    decimal PlatformRevenue,
    int TotalShipments,
    int TotalOrderItems,
    int TotalProducts,
    int TotalSellers,
    int TotalCustomers,
    int TotalDrivers,
    int TotalSellerOrders,
    int TotalFactoryOrders,
    decimal CommissionPercentage
);
