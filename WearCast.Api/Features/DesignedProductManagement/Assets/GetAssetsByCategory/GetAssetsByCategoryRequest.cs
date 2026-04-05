namespace WearCast.Api.Features.DesignedProductManagement.Assets.GetAssetsByCategory
{
    public record GetAssetsByCategoryRequest(int CategoryId) : IRequest<Result<IEnumerable<GetAssetsByCategoryResponse>>>;
}
