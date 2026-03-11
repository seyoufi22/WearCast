namespace WearCast.Api.Features.DesignedProductManagement.FactoryProducts.DeleteDesignedProduct
{
    public record DeleteDesignedProductRequest(string Slug) : IRequest<Result>;
}
