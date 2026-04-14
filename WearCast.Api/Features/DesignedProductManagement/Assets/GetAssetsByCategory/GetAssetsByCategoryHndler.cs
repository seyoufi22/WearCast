using WearCast.Api.Common.Helper;
using WearCast.Api.Common.Views;
using WearCast.Api.Features.DesignedProductManagement.AssetsCategories;

namespace WearCast.Api.Features.DesignedProductManagement.Assets.GetAssetsByCategory
{
    public class GetAssetsByCategoryHndler(ApplicationDbContext context) : IRequestHandler<GetAssetsByCategoryRequest, Result<PagingViewModel<GetAssetsByCategoryResponse>>>
    {
        private readonly ApplicationDbContext _context = context;

        public async Task<Result<PagingViewModel<GetAssetsByCategoryResponse>>> Handle(GetAssetsByCategoryRequest request, CancellationToken cancellationToken)
        {
            var categoryExists = await _context.DesignAssetCategories
                .AnyAsync(c => c.Id == request.CategoryId, cancellationToken);

            if (!categoryExists)
            {
                return Result.Failure<PagingViewModel<GetAssetsByCategoryResponse>>(AssetsCategoryErrors.CategoryNotFound);
            }

            var query = _context.DesignAssets
                .AsNoTracking()
                .Where(a => a.DesignAssetCategoryId == request.CategoryId)
                .Select(a => new GetAssetsByCategoryResponse(
                    a.Id,
                    a.Name,
                    a.ImageUrl,
                    a.WidthPx,
                    a.HeightPx,
                    a.DesignAssetCategoryId
                ));


            var pagedResult = await PagingHelper.CreateAsync(query, request.PageIndex, request.PageSize);

            return Result.Success(pagedResult);

        }
    }
}
