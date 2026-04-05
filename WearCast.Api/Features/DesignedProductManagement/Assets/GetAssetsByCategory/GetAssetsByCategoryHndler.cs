using WearCast.Api.Features.DesignedProductManagement.AssetsCategories;

namespace WearCast.Api.Features.DesignedProductManagement.Assets.GetAssetsByCategory
{
    public class GetAssetsByCategoryHndler(ApplicationDbContext context) : IRequestHandler<GetAssetsByCategoryRequest, Result<IEnumerable<GetAssetsByCategoryResponse>>>
    {
        private readonly ApplicationDbContext _context = context;

        public async Task<Result<IEnumerable<GetAssetsByCategoryResponse>>> Handle(GetAssetsByCategoryRequest request, CancellationToken cancellationToken)
        {
            var categoryExists = await _context.DesignAssetCategories
                .AnyAsync(c => c.Id == request.CategoryId, cancellationToken);

            if (!categoryExists)
            {
                return Result.Failure<IEnumerable<GetAssetsByCategoryResponse>>(AssetsCategoryErrors.CategoryNotFound);
            }

            var assets = await _context.DesignAssets
                .AsNoTracking()
                .Where(a => a.DesignAssetCategoryId == request.CategoryId)
                .Select(a => new GetAssetsByCategoryResponse(
                   a.Id,
                    a.Name,
                    a.ImageUrl,
                    a.WidthPx,
                    a.HeightPx,
                    a.DesignAssetCategoryId
                    ))
                .ToListAsync(cancellationToken);

            return Result.Success<IEnumerable<GetAssetsByCategoryResponse>>(assets);
        }
    }
}
