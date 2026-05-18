namespace WearCast.Api.Features.DesignedProductManagement.CustomerCatalogAndWorkspace.GetRecommendedProductsForCustomer
{
    public class GetRecommendedProductsForCustomerRequest : IRequest<Result<List<GetRecommendedProductsForCustomerResponse>>>
    {
        public int TopK { get; set; } = 10;
    }

    public class GetRecommendedProductsForCustomerResponse
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public decimal Price { get; set; }
        public string? CategoryName { get; set; }
        public string? MainImageUrl { get; set; }
    }
}
