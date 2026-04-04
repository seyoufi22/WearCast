using WearCast.Api.Features.DesignedProductManagement.AssetsCategories;

namespace WearCast.Api.Features.DesignedProductManagement.Assets.GetAllAssets
{
    public class GetAllAssetsHndler(ApplicationDbContext context) : IRequestHandler<GetAllAssetsRequest, Result<IEnumerable<GetAllAssetsResponse>>>
    {
        private readonly ApplicationDbContext _context = context;

        public async Task<Result<IEnumerable<GetAllAssetsResponse>>> Handle(GetAllAssetsRequest request, CancellationToken cancellationToken)
        {
            var categoryExists = await _context.DesignAssetCategories
                .AnyAsync(c => c.Id == request.CategoryId, cancellationToken);

            if (!categoryExists)
            {
                return Result.Failure<IEnumerable<GetAllAssetsResponse>>(AssetsCategoryErrors.CategoryNotFound);
            }

            var assets = await _context.DesignAssets
                .AsNoTracking()
                .Where(a => a.DesignAssetCategoryId == request.CategoryId)
                .Select(a => new GetAllAssetsResponse(
                   a.Id,
                    a.Name,
                    a.ImageUrl,
                    a.WidthPx,
                    a.HeightPx,
                    a.DesignAssetCategoryId
                    ))
                .ToListAsync(cancellationToken);

            return Result.Success<IEnumerable<GetAllAssetsResponse>>(assets);
        }
    }
}
