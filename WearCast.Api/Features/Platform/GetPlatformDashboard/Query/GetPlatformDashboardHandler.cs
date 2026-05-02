using WearCast.Api.Common.Enums;

using WearCast.Api.Features.Platform.GetPlatformDashboard.DTOs;

namespace WearCast.Api.Features.Platform.GetPlatformDashboard.Query;

public class GetPlatformDashboardHandler(ApplicationDbContext context)
    : IRequestHandler<GetPlatformDashboardRequest, Result<GetPlatformDashboardResponse>>
{
    public async Task<Result<GetPlatformDashboardResponse>> Handle(
        GetPlatformDashboardRequest request, CancellationToken cancellationToken)
    {
        // Paid orders only (excluding deleted)
        var paidOrders = context.Orders
            .AsNoTracking()
            .Where(o => !o.IsDeleted && (o.Status == OrderStatus.Paid || o.Status == OrderStatus.Ready || o.Status == OrderStatus.PickedUp));

        var totalMoneyProcessed = await paidOrders.SumAsync(o => o.TotalAmount, cancellationToken);
        var platformRevenue = await paidOrders.SumAsync(o => o.Commission, cancellationToken);
        var totalSellerOrders = await paidOrders.CountAsync(o => o.SellerId != null, cancellationToken);
        var totalFactoryOrders = await paidOrders.CountAsync(o => o.FactoryId != null, cancellationToken);



        // Counts (excluding deleted)
        var totalShipments = await context.Shipments.AsNoTracking().CountAsync(s => !s.IsDeleted, cancellationToken);

        var totalFixedOrderItems = await context.FixedProductOrderItems.AsNoTracking().CountAsync(i => !i.IsDeleted, cancellationToken);
        var totalDesignedOrderItems = await context.CustomerDesignedOrderItems.AsNoTracking().CountAsync(i => !i.IsDeleted, cancellationToken);
        var totalOrderItems = totalFixedOrderItems + totalDesignedOrderItems;

        var totalFixedProducts = await context.FixedProducts.AsNoTracking().CountAsync(p => !p.IsDeleted, cancellationToken);
        var totalDesignedProducts = await context.DesignedProducts.AsNoTracking().CountAsync(p => !p.IsDeleted, cancellationToken);
        var totalProducts = totalFixedProducts + totalDesignedProducts;

        var totalSellers = await context.Sellers.AsNoTracking().CountAsync(s => !s.IsDeleted, cancellationToken);
        var totalCustomers = await context.Customers.AsNoTracking().CountAsync(c => !c.IsDeleted, cancellationToken);
        var totalDrivers = await context.Drivers.AsNoTracking().CountAsync(d => !d.IsDeleted, cancellationToken);

        // Commission percentage
        var settings = await context.PlatformSettings.AsNoTracking().FirstOrDefaultAsync(cancellationToken);
        var commissionPercentage = settings?.CommissionPercentage ?? 2m;

        return Result.Success(new GetPlatformDashboardResponse(
            TotalMoneyProcessed: totalMoneyProcessed,
            PlatformRevenue: platformRevenue,
            TotalShipments: totalShipments,
            TotalOrderItems: totalOrderItems,
            TotalProducts: totalProducts,
            TotalSellers: totalSellers,
            TotalCustomers: totalCustomers,
            TotalDrivers: totalDrivers,
            TotalSellerOrders: totalSellerOrders,
            TotalFactoryOrders: totalFactoryOrders,
            CommissionPercentage: commissionPercentage
        ));
    }
}
