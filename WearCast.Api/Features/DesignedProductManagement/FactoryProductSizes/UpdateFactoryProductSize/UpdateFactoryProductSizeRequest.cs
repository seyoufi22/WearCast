namespace WearCast.Api.Features.DesignedProductManagement.FactoryProductSizes.UpdateFactoryProductSize
{
    public record UpdateFactoryProductSizeRequest(
        int Id,
        decimal? A,
        decimal? B,
        decimal? C
    ) : IRequest<Result>;
}
