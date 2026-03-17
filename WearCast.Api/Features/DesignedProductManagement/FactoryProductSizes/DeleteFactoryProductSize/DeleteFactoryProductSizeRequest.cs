namespace WearCast.Api.Features.DesignedProductManagement.FactoryProductSizes.DeleteFactoryProductSize
{
    public record DeleteFactoryProductSizeRequest(
        int Id
        ) : IRequest<Result>;
}
