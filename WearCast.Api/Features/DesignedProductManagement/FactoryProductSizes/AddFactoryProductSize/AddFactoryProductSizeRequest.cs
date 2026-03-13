namespace WearCast.Api.Features.DesignedProductManagement.FactoryProductSizes.AddFactoryProductSize
{
    public record AddFactoryProductSizeRequest(
        Size Size,
        decimal? A,
        decimal? B,
        decimal? C,
        int ProductId
    ) : IRequest<Result>;
}
