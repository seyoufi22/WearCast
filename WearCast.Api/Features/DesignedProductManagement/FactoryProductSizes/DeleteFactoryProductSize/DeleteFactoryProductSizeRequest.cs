namespace WearCast.Api.Features.DesignedProductManagement.FactoryProductSizes.DeleteFactoryProductSize
{
    public record DeleteFactoryProductSizeRequest(
        string ProductSlug,
        Size Size
        ) : IRequest<Result>;
}
