namespace WearCast.Api.Features.Factories.FactoryDashboard
{
    public class GetFactoryDashboardMetricsHandler(
        ApplicationDbContext context,
        IHttpContextAccessor httpContextAccessor
        ) : IRequestHandler<GetFactoryDashboardMetricsRequest, Result<GetFactoryDashboardMetricsResponse>>
    {
        private readonly ApplicationDbContext _context = context;
        private readonly IHttpContextAccessor _httpContextAccessor = httpContextAccessor;

        public async Task<Result<GetFactoryDashboardMetricsResponse>> Handle(GetFactoryDashboardMetricsRequest request, CancellationToken cancellationToken)
        {
            var user = _httpContextAccessor.HttpContext!.User;

            if (!user.IsFactoryManager())
            {
                return Result.Failure<GetFactoryDashboardMetricsResponse>(AuthErrors.Forbidden);
            }

            var factoryId = user.GetFactoryId();
            if (factoryId == null)
            {
                return Result.Failure<GetFactoryDashboardMetricsResponse>(AuthErrors.NoAssociatedFactory);
            }

            var totalProducts = await _context.DesignedProducts
                .CountAsync(p => p.FactoryId == factoryId.Value, cancellationToken);

            var totalOrders = await _context.Orders
                .CountAsync(o => o.FactoryId == factoryId.Value, cancellationToken);

            var totalUniqueCustomers = await _context.Orders
                .Where(o => o.FactoryId == factoryId.Value)
                .Select(o => o.CustomerId)
                .Distinct()
                .CountAsync(cancellationToken);

            var response = new GetFactoryDashboardMetricsResponse(
                TotalProducts: totalProducts,
                TotalOrders: totalOrders,
                TotalUniqueCustomers: totalUniqueCustomers
            );

            return Result.Success(response);
        }
    }
}
