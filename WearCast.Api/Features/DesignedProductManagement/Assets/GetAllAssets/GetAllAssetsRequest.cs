using WearCast.Api.Common.Views;

namespace WearCast.Api.Features.DesignedProductManagement.Assets.GetAllAssets
{
    public record GetAllAssetsRequest(
        int PageIndex = 1,
        int PageSize = 10,
        int? CategoryId = null,
        string? SearchTerm = null
        ) : IRequest<Result<PagingViewModel<GetAllAssetsResponse>>>;
}
