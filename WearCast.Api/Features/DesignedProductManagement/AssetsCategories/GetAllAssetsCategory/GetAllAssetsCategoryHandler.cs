namespace WearCast.Api.Features.DesignedProductManagement.AssetsCategories.GetAllAssetsCategory
{
    public class GetAllAssetsCategoryHandler(ApplicationDbContext context) : IRequestHandler<GetAllAssetsCategoryRequest, Result<IEnumerable<GetAllAssetsCategoryResponse>>>
    {
        private readonly ApplicationDbContext _context = context;

        public async Task<Result<IEnumerable<GetAllAssetsCategoryResponse>>> Handle(GetAllAssetsCategoryRequest request, CancellationToken cancellationToken)
        {
            var categories = await _context.DesignAssetCategories
                .AsNoTracking()
                .Select(c => new GetAllAssetsCategoryResponse(
                    c.Id,
                    c.Name
                    ))
                .ToListAsync(cancellationToken);

            return Result.Success<IEnumerable<GetAllAssetsCategoryResponse>>(categories);
        }
    }
}

