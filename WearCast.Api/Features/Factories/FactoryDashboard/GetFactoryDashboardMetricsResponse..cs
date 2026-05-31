namespace WearCast.Api.Features.Factories.FactoryDashboard
{
    public record GetFactoryDashboardMetricsResponse(
         int TotalProducts,
         int TotalOrders,
         int TotalUniqueCustomers
     );
}
