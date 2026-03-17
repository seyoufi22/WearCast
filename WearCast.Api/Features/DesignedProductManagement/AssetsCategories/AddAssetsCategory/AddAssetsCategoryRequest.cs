namespace WearCast.Api.Features.DesignedProductManagement.AssetsCategories.AddAssetsCategory
{
    public record AddAssetsCategoryRequest(string Name) : IRequest<Result<AddAssetsCategoryResponse>>;
}
