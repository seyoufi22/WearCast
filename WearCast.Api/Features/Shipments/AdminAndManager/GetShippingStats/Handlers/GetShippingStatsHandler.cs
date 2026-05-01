using Microsoft.AspNetCore.Http;
using System.Security.Claims;
using WearCast.Api.Features.Shipments.AdminAndManager.GetShippingStats.DTOs;

namespace WearCast.Api.Features.Shipments.AdminAndManager.GetShippingStats.Handlers
{
    public class GetShippingStatsHandler(
        ApplicationDbContext context,
        IHttpContextAccessor httpContextAccessor
    ) : IRequestHandler<GetShippingStatsRequest, Result<ShippingStatsResponse>>
    {
        private readonly ApplicationDbContext _context = context;
        private readonly IHttpContextAccessor _httpContextAccessor = httpContextAccessor;

        public async Task<Result<ShippingStatsResponse>> Handle(GetShippingStatsRequest request, CancellationToken cancellationToken)
        {
            var user = _httpContextAccessor.HttpContext!.User;
            int? companyId = user.GetShippingCompanyId();

            if (!companyId.HasValue && !user.IsSuperAdmin())
            {
                return Result.Failure<ShippingStatsResponse>(new Error("Auth.NoCompany", "Shipping company context not found.", 403));
            }

            // If SuperAdmin, we might want to allow providing a companyId or just show global stats.
            // For now, let's assume we filter by companyId if provided or if the user is a manager.
            var effectiveCompanyId = companyId;

            var shipmentsQuery = _context.Shipments.AsNoTracking();
            var driversQuery = _context.Drivers.AsNoTracking();

            if (effectiveCompanyId.HasValue)
            {
                shipmentsQuery = shipmentsQuery.Where(s => s.ShippingCompanyId == effectiveCompanyId.Value);
                driversQuery = driversQuery.Where(d => d.ShippingCompanyId == effectiveCompanyId.Value);
            }

            var now = DateTime.UtcNow;
            var currentMonthStart = new DateTime(now.Year, now.Month, 1);
            var lastMonthStart = currentMonthStart.AddMonths(-1);

            // Basic Stats
            var totalShipments = await shipmentsQuery.CountAsync(cancellationToken);
            var activeDrivers = await driversQuery.CountAsync(d => d.Status == DriverStatus.Available, cancellationToken);
            var totalRevenue = await shipmentsQuery
                .Where(s => s.ShipmentStatus == ShipmentStatus.Delivered)
                .SumAsync(s => s.Price, cancellationToken);
            var pendingDeliveries = await shipmentsQuery.CountAsync(s => 
                s.ShipmentStatus == ShipmentStatus.Pending || 
                s.ShipmentStatus == ShipmentStatus.Unassigned ||
                s.ShipmentStatus == ShipmentStatus.Assigned ||
                s.ShipmentStatus == ShipmentStatus.PickingUp, cancellationToken);

            // Growth Calculations
            var currentMonthShipmentsCount = await shipmentsQuery.CountAsync(s => s.CreatedOn >= currentMonthStart, cancellationToken);
            var lastMonthShipmentsCount = await shipmentsQuery.CountAsync(s => s.CreatedOn >= lastMonthStart && s.CreatedOn < currentMonthStart, cancellationToken);
            var shipmentsGrowth = lastMonthShipmentsCount == 0 ? (currentMonthShipmentsCount > 0 ? 100 : 0) : Math.Round((double)(currentMonthShipmentsCount - lastMonthShipmentsCount) / lastMonthShipmentsCount * 100, 1);

            var currentMonthRevenue = await shipmentsQuery
                .Where(s => s.ShipmentStatus == ShipmentStatus.Delivered && s.CreatedOn >= currentMonthStart)
                .SumAsync(s => s.Price, cancellationToken);
            var lastMonthRevenue = await shipmentsQuery
                .Where(s => s.ShipmentStatus == ShipmentStatus.Delivered && s.CreatedOn >= lastMonthStart && s.CreatedOn < currentMonthStart)
                .SumAsync(s => s.Price, cancellationToken);
            var revenueGrowth = lastMonthRevenue == 0 ? (currentMonthRevenue > 0 ? 100 : 0) : Math.Round((double)(currentMonthRevenue - lastMonthRevenue) / (double)lastMonthRevenue * 100, 1);

            // Monthly Revenue (Last 6 Months)
            var monthlyRevenue = new List<MonthlyRevenueDto>();
            for (int i = 5; i >= 0; i--)
            {
                var monthStart = currentMonthStart.AddMonths(-i);
                var monthEnd = monthStart.AddMonths(1);
                var revenue = await shipmentsQuery
                    .Where(s => s.ShipmentStatus == ShipmentStatus.Delivered && s.CreatedOn >= monthStart && s.CreatedOn < monthEnd)
                    .SumAsync(s => s.Price, cancellationToken);
                
                monthlyRevenue.Add(new MonthlyRevenueDto(monthStart.ToString("MMM"), revenue));
            }

            // Status Breakdown
            var statusBreakdown = await shipmentsQuery
                .GroupBy(s => s.ShipmentStatus)
                .Select(g => new { Status = g.Key.ToString(), Count = g.Count() })
                .ToDictionaryAsync(x => x.Status, x => x.Count, cancellationToken);

            // Active Drivers Growth (Note: Driver model lacks CreatedOn, so growth is set to 0)
            var activeDriversCount = await driversQuery.CountAsync(d => d.Status == DriverStatus.Available, cancellationToken);
            var activeDriversGrowth = 0;

            // Pending Growth
            var currentMonthPending = await shipmentsQuery.CountAsync(s => 
                (s.ShipmentStatus == ShipmentStatus.Pending || s.ShipmentStatus == ShipmentStatus.Unassigned) && s.CreatedOn >= currentMonthStart, cancellationToken);
            var lastMonthPending = await shipmentsQuery.CountAsync(s => 
                (s.ShipmentStatus == ShipmentStatus.Pending || s.ShipmentStatus == ShipmentStatus.Unassigned) && s.CreatedOn >= lastMonthStart && s.CreatedOn < currentMonthStart, cancellationToken);
            var pendingGrowth = lastMonthPending == 0 ? (currentMonthPending > 0 ? 100 : 0) : Math.Round((double)(currentMonthPending - lastMonthPending) / lastMonthPending * 100, 1);

            var response = new ShippingStatsResponse(
                totalShipments,
                activeDrivers,
                totalRevenue,
                pendingDeliveries,
                shipmentsGrowth,
                activeDriversGrowth,
                revenueGrowth,
                pendingGrowth,
                monthlyRevenue,
                statusBreakdown
            );

            return Result.Success(response);
        }
    }
}
