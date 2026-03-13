namespace WearCast.Api.Features.DesignedProductManagement.FactoryProductImages.DeleteFactoryProductImage
{
    public record DeleteFactoryProductImageRequest(
         int ImageId
        ) : IRequest<Result>;
}
