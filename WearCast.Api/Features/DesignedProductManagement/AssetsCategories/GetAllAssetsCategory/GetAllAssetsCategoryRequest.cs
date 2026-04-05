namespace WearCast.Api.Features.DesignedProductManagement.AssetsCategories.GetAllAssetsCategory
{
    public record GetAllAssetsCategoryRequest() : IRequest<Result<IEnumerable<GetAllAssetsCategoryResponse>>>;
}
