namespace WearCast.Api.Features.DesignedProductManagement.FactoryProductColors.DeleteFactoryProductColor
{
    public record DeleteFactoryProductColorRequest(
        string ProductSlug,
        string CurrentColorSlug
        ) : IRequest<Result>;
}
