namespace WearCast.Api.Features.DesignedProductManagement.Assets.GetAllAssets
{
    public record GetAllAssetsResponse(
        int Id,
        string Name,
        string ImageUrl,
        int WidthPx,
        int HeightPx,
        int CategoryId);
}
