namespace WearCast.Api.Features.DesignedProductManagement.Assets.GetAsset
{
    public record GetAssetResponse(
        int Id,
        string Name,
        string ImageUrl,
        int WidthPx,
        int HeightPx,
        int CategoryId
        );
}
