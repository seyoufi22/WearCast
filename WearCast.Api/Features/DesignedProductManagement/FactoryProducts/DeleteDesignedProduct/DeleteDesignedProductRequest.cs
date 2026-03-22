namespace WearCast.Api.Features.DesignedProductManagement.FactoryProducts.DeleteDesignedProduct
{
    public record DeleteDesignedProductRequest(int Id) : IRequest<Result>;
}
