namespace WearCast.Api.Features.DesignedProductManagement.FactoryProductSizes.AddFactoryProductSize
{
    public record AddFactoryProductSizeRequest(
        string ProductSlug,
        Size Size,
        decimal? A,
        decimal? B,
        decimal? C
    ) : IRequest<Result>;
}
