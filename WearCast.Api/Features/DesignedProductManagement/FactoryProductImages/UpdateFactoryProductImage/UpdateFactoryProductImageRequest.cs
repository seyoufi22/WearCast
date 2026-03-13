namespace WearCast.Api.Features.DesignedProductManagement.FactoryProductImages.UpdateFactoryProductImage
{
    public record UpdateFactoryProductImageRequest(
        int ImageId,
        IFormFile? Image,
        ViewSide ViewSide
    ) : IRequest<Result>;
}
