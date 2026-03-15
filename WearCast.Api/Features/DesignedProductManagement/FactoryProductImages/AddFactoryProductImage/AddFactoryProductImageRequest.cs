namespace WearCast.Api.Features.DesignedProductManagement.FactoryProductImages.AddFactoryProductImage
{
    public record AddFactoryProductImageRequest(
         int ColorId,
         IFormFile Image,
         ViewSide ViewSide
     ) : IRequest<Result<FactoryProductImagesResponse>>;
}
