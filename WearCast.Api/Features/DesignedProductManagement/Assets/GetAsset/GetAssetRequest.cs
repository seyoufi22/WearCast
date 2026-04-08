namespace WearCast.Api.Features.DesignedProductManagement.Assets.GetAsset
{
    public record GetAssetRequest(int Id) : IRequest<Result<GetAssetResponse>>;
}
