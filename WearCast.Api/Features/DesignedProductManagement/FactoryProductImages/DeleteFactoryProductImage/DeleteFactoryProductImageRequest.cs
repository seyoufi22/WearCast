namespace WearCast.Api.Features.DesignedProductManagement.FactoryProductImages.DeleteFactoryProductImage
{
    public record DeleteFactoryProductImageRequest(
         string ColorSlug,
         int ImageId
        ) : IRequest<Result>;
}
