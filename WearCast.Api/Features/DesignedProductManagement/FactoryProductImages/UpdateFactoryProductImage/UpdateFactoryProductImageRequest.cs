namespace WearCast.Api.Features.DesignedProductManagement.FactoryProductImages.UpdateFactoryProductImage
{
    public record UpdateFactoryProductImageRequest(
        string ColorSlug,
        int ImageId,
        IFormFile? Image,
        ViewSide ViewSide
        ) : IRequest<Result>;
}
