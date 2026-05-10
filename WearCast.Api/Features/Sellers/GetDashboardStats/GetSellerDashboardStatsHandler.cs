using WearCast.Api.Common.Enums;
using WearCast.Api.Features.Common.DTOs;
using Microsoft.EntityFrameworkCore;
using MediatR;

namespace WearCast.Api.Features.Sellers.GetDashboardStats;

public class GetSellerDashboardStatsHandler(
    ApplicationDbContext context,
    IHttpContextAccessor httpContextAccessor
) : IRequestHandler<GetSellerDashboardStatsRequest, Result<SellerDashboardStatsResponse>>
{
    public async Task<Result<SellerDashboardStatsResponse>> Handle(GetSellerDashboardStatsRequest request, CancellationToken cancellationToken)
    {
        var user = httpContextAccessor.HttpContext!.User;
        var sellerId = user.GetSellerId();

        if (sellerId == null)
            return Result.Failure<SellerDashboardStatsResponse>(new Error("Seller.NotFound", "Seller not found in token.", StatusCodes.Status404NotFound));

        var orderStats = await context.Orders
            .AsNoTracking()
            .Where(o => o.SellerId == sellerId.Value)
            .GroupBy(o => 1)
            .Select(g => new
            {
                TotalRevenue = g.Sum(o => o.Payout),
                TotalOrders = g.Count(),
                PendingOrders = g.Count(o => o.Status == OrderStatus.Pending)
            })
            .FirstOrDefaultAsync(cancellationToken);

        var uniqueProductsCount = await context.FixedProducts
            .AsNoTracking()
            .Where(p => p.SellerId == sellerId.Value)
            .CountAsync(cancellationToken);

        
        var sizesLists = await context.FixedProducts
            .AsNoTracking()
            .Where(p => p.SellerId == sellerId.Value)
            .SelectMany(p => p.Colors)
            .Select(c => c.Sizes) 
            .ToListAsync(cancellationToken);

        var totalInventoryItems = sizesLists
            .SelectMany(sizes => sizes) 
            .Sum(s => s.Quantity);      

        var response = new SellerDashboardStatsResponse(
            TotalRevenue: orderStats?.TotalRevenue ?? 0m,
            TotalOrders: orderStats?.TotalOrders ?? 0,
            PendingOrders: orderStats?.PendingOrders ?? 0,
            UniqueProductsCount: uniqueProductsCount,
            TotalInventoryItems: totalInventoryItems
        );

        return Result.Success(response);
    }
}