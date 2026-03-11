namespace WearCast.Api.Features.DesignedProductManagement.FactoryProductSizes.UpdateFactoryProductSize
{
    public record UpdateFactoryProductSizeRequest(
        string ProductSlug,
        Size Size,
        decimal? A,
        decimal? B,
        decimal? C
    ) : IRequest<Result>;
}
