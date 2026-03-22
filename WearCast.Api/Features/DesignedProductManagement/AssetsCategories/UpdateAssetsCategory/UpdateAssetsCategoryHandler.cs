namespace WearCast.Api.Features.DesignedProductManagement.AssetsCategories.UpdateAssetsCategory
{
    public class UpdateAssetsCategoryHandler(ApplicationDbContext context) : IRequestHandler<UpdateAssetsCategoryRequest, Result>
    {
        private readonly ApplicationDbContext _context = context;

        public async Task<Result> Handle(UpdateAssetsCategoryRequest request, CancellationToken cancellationToken)
        {
            var category = await _context.DesignAssetCategories
                .FirstOrDefaultAsync(c => c.Id == request.Id, cancellationToken);

            if (category == null)
            {
                return Result.Failure(AssetsCategoryErrors.CategoryNotFound);
            }

            var normalizedName = request.Name.Trim();

            if (category.Name != normalizedName)
            {
                var nameExists = await _context.DesignAssetCategories
                    .AnyAsync(c => c.Name == normalizedName && c.Id != request.Id, cancellationToken);

                if (nameExists)
                {
                    return Result.Failure(AssetsCategoryErrors.CategoryAlreadyExists);
                }

                category.Name = normalizedName;
            }

            await _context.SaveChangesAsync(cancellationToken);

            return Result.Success();
        }
    }
}
