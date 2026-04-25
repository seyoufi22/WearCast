namespace WearCast.Api.Features.DesignedProductManagement.Assets.GetAllAssets
{
    public record GetAllAssetsResponse(
        int Id,
        string Name,
        string ImageUrl,
        int CategoryId,
        string CategoryName
        );
}
