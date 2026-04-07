namespace WearCast.Api.Features.DesignedProductManagement.Assets.GetAssetsByCategory
{
    public record GetAssetsByCategoryResponse(
        int Id,
        string Name,
        string ImageUrl,
        int WidthPx,
        int HeightPx,
        int CategoryId);
}
