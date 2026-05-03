using WearCast.Api.Common.Helper;
using WearCast.Api.Common.Views;

namespace WearCast.Api.Features.DesignedProductManagement.Assets.GetAllAssets
{
    public class GetAllAssetsHandler(
        ApplicationDbContext context
        ) : IRequestHandler<GetAllAssetsRequest, Result<PagingViewModel<GetAllAssetsResponse>>>
    {
        public async Task<Result<PagingViewModel<GetAllAssetsResponse>>> Handle(GetAllAssetsRequest request, CancellationToken cancellationToken)
        {
            var query = context.DesignAssets.AsNoTracking();

            if (!string.IsNullOrWhiteSpace(request.SearchTerm))
            {
                var search = request.SearchTerm.Trim().ToLower();
                query = query.Where(a =>
                    a.Name.ToLower().Contains(search) ||
                    a.DesignAssetCategory.Name.ToLower().Contains(search)
                );
            }

            if (request.CategoryId.HasValue)
            {
                query = query.Where(a => a.DesignAssetCategoryId == request.CategoryId.Value);
            }

            var projectedQuery = query
                .OrderByDescending(a => a.Id)
                .Select(a => new GetAllAssetsResponse(
                    a.Id,
                    a.Name,
                    a.ImageUrl,
                    a.WidthPx,
                    a.HeightPx,
                    a.DesignAssetCategoryId,
                    a.DesignAssetCategory.Name
                ));

            var pagedResult = await PagingHelper.CreateAsync(projectedQuery, request.PageIndex, request.PageSize);

            return Result.Success(pagedResult);
        }
    }
}
