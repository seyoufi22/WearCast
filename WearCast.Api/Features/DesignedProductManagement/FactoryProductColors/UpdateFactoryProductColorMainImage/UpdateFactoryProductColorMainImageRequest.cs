namespace WearCast.Api.Features.DesignedProductManagement.FactoryProductColors.UpdateFactoryProductColorMainImage
{
    public record UpdateFactoryProductColorMainImageRequest(IFormFile Image, int ProductId, int ColorId) : IRequest<Result>;
}
