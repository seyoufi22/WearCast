namespace WearCast.Api.Features.DesignedProductManagement.AssetsCategories.UpdateAssetsCategory
{
    public record UpdateAssetsCategoryRequest(
        int Id,
        string Name
        ) : IRequest<Result>;
}
