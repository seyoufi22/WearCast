namespace WearCast.Api.Features.DesignedProductManagement.FactoryProductImages.UpsertFactoryProductImage
{
    public record UpsertFactoryProductImageRequest(
        int ColorId,
        ViewSide ViewSide,
        IFormFile NewImage
    ) : IRequest<Result<FactoryProductImagesResponse>>;
}
