namespace WearCast.Api.Features.DesignedProductManagement.Assets.GetAllAssets
{
    public record GetAllAssetsRequest(int CategoryId) : IRequest<Result<IEnumerable<GetAllAssetsResponse>>>;
}
