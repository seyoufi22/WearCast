namespace WearCast.Api.Features.DesignedProductManagement.FactoryProductImages.AddFactoryProductImage
{
    public record AddFactoryProductImageRequest(
         string ColorSlug,
         IFormFile Image,
         ViewSide ViewSide
     ) : IRequest<Result>;
}
