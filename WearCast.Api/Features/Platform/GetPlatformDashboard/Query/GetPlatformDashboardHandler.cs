using WearCast.Api.Common.Enums;

using WearCast.Api.Features.Platform.GetPlatformDashboard.DTOs;

namespace WearCast.Api.Features.Platform.GetPlatformDashboard.Query;

public class GetPlatformDashboardHandler(ApplicationDbContext context)
    : IRequestHandler<GetPlatformDashboardRequest, Result<GetPlatformDashboardResponse>>
{
    public async Task<Result<GetPlatformDashboardResponse>> Handle(
        GetPlatformDashboardRequest request, CancellationToken cancellationToken)
    {
        // Paid orders only
        var paidOrders = context.Orders
            .AsNoTracking()
            .Where(o => o.Status == OrderStatus.Paid || o.Status == OrderStatus.Ready || o.Status == OrderStatus.PickedUp);

        var totalMoneyProcessed = await paidOrders.SumAsync(o => o.TotalAmount, cancellationToken);
        var platformRevenue = await paidOrders.SumAsync(o => o.Commission, cancellationToken);
        var totalOrders = await paidOrders.CountAsync(cancellationToken);

        // Platform wallet balance
        var platformWallet = await context.Wallets
            .AsNoTracking()
            .Where(w => w.OwnerType == WalletOwnerType.Platform)
            .FirstOrDefaultAsync(cancellationToken);
        var platformWalletBalance = platformWallet?.Balance ?? 0m;

        // Counts
        var totalShipments = await context.Shipments.AsNoTracking().CountAsync(cancellationToken);

        var totalFixedOrderItems = await context.FixedProductOrderItems.AsNoTracking().CountAsync(cancellationToken);
        var totalDesignedOrderItems = await context.CustomerDesignedOrderItems.AsNoTracking().CountAsync(cancellationToken);
        var totalOrderItems = totalFixedOrderItems + totalDesignedOrderItems;

        var totalFixedProducts = await context.FixedProducts.AsNoTracking().CountAsync(cancellationToken);
        var totalDesignedProducts = await context.DesignedProducts.AsNoTracking().CountAsync(cancellationToken);
        var totalProducts = totalFixedProducts + totalDesignedProducts;

        var totalSellers = await context.Sellers.AsNoTracking().CountAsync(cancellationToken);
        var totalCustomers = await context.Customers.AsNoTracking().CountAsync(cancellationToken);

        // Commission percentage
        var settings = await context.PlatformSettings.AsNoTracking().FirstOrDefaultAsync(cancellationToken);
        var commissionPercentage = settings?.CommissionPercentage ?? 2m;

        return Result.Success(new GetPlatformDashboardResponse(
            TotalMoneyProcessed: totalMoneyProcessed,
            PlatformRevenue: platformRevenue,
            PlatformWalletBalance: platformWalletBalance,
            TotalShipments: totalShipments,
            TotalOrderItems: totalOrderItems,
            TotalProducts: totalProducts,
            TotalSellers: totalSellers,
            TotalCustomers: totalCustomers,
            TotalOrders: totalOrders,
            CommissionPercentage: commissionPercentage
        ));
    }
}
