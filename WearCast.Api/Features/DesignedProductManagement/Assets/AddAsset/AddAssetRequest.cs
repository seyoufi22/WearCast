namespace WearCast.Api.Features.DesignedProductManagement.Assets.AddAsset
{
    public record AddAssetRequest(
        string Name,
        IFormFile Image,
        int WidthPx,
        int HeightPx,
        int CategoryId
        ) : IRequest<Result<AddAssetResponse>>;
}
