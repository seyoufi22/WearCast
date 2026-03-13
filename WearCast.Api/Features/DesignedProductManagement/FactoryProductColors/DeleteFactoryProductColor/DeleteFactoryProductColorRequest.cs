namespace WearCast.Api.Features.DesignedProductManagement.FactoryProductColors.DeleteFactoryProductColor
{
    public record DeleteFactoryProductColorRequest(
        int ColorId,
        int ProductId
        ) : IRequest<Result>;
}
