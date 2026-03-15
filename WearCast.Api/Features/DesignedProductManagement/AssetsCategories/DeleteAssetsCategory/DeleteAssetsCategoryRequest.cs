namespace WearCast.Api.Features.DesignedProductManagement.AssetsCategories.DeleteAssetsCategory
{
    public record DeleteAssetsCategoryRequest(int Id) : IRequest<Result>;
}
