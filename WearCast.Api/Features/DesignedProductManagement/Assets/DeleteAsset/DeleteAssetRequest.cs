namespace WearCast.Api.Features.DesignedProductManagement.Assets.DeleteAsset
{
    public record DeleteAssetRequest(
        int Id
        ) : IRequest<Result>;
}
