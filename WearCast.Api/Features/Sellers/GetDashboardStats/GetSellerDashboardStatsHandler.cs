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

        var topProductsRaw = await context.FixedProducts
            .AsNoTracking()
            .Where(p => p.SellerId == sellerId.Value && !p.IsDeleted)
            .OrderByDescending(p => p.SalesCount) 
            .Take(5)
            .Select(p => new
            {
                p.Id,
                p.Name,
                p.Price,
                p.TargetAudience,
                p.SalesCount,
                p.Category.ImageUrl,
                ActiveColors = p.Colors.Where(c => !c.IsDeleted).ToList()
            })
            .ToListAsync(cancellationToken);

        
        var topSellingProducts = topProductsRaw.Select(p =>
        {
            var totalStock = p.ActiveColors.SelectMany(c => c.Sizes).Sum(s => s.Quantity);
            var mainImageUrl = p.ActiveColors
                .Where(c => c.Sizes.Any(s => s.Quantity > 0))
                .OrderBy(c => c.Id)
                .Select(c => c.ImageUrl)
                .FirstOrDefault() ?? p.ImageUrl;

            return new TopSellingProductDto(
                ProductId: p.Id,
                Name: p.Name,
                Price: p.Price,
                TargetAudience: p.TargetAudience,
                Stock: totalStock,
                IsRejected: !p.ActiveColors.Any(),
                MainImageUrl: mainImageUrl,
                TotalSold: p.SalesCount 
            );
        }).ToList();

        var response = new SellerDashboardStatsResponse(
            TotalRevenue: orderStats?.TotalRevenue ?? 0m,
            TotalOrders: orderStats?.TotalOrders ?? 0,
            PendingOrders: orderStats?.PendingOrders ?? 0,
            UniqueProductsCount: uniqueProductsCount,
            TotalInventoryItems: totalInventoryItems,
            TopSellingProducts: topSellingProducts
        );

        return Result.Success(response);
    }
}