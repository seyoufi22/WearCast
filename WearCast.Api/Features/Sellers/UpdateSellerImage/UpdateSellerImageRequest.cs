namespace WearCast.Api.Features.Sellers.UpdateSellerImage
{
    public record UpdateSellerImageRequest(IFormFile NewLogo, int? ProvidedSellerId = null) : IRequest<Result>;
}
