namespace WearCast.Api.Features.DesignedProductManagement.AssetsCategories.DeleteAssetsCategory
{
    public class DeleteAssetsCategoryHandler(ApplicationDbContext context) : IRequestHandler<DeleteAssetsCategoryRequest, Result>
    {
        private readonly ApplicationDbContext _context = context;

        public async Task<Result> Handle(DeleteAssetsCategoryRequest request, CancellationToken cancellationToken)
        {
            var category = await _context.DesignAssetCategories
                .FirstOrDefaultAsync(c => c.Id == request.Id, cancellationToken);

            if (category == null)
            {
                return Result.Failure(AssetsCategoryErrors.CategoryNotFound);
            }

            category.IsDeleted = true;

            await _context.SaveChangesAsync(cancellationToken);

            return Result.Success();
        }
    }
}
