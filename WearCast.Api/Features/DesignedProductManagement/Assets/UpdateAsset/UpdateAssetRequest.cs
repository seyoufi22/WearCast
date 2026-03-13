namespace WearCast.Api.Features.DesignedProductManagement.Assets.UpdateAsset
{
    public record UpdateAssetRequest(
        int Id,
        string Name,
        int WidthPx,
        int HeightPx,
        IFormFile? Image,
        int CategoryId
        ) : IRequest<Result>;
}
