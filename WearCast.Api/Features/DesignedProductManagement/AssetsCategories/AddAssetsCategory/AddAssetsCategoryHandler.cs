namespace WearCast.Api.Features.DesignedProductManagement.AssetsCategories.AddAssetsCategory
{
    public class AddAssetsCategoryHandler(ApplicationDbContext context) : IRequestHandler<AddAssetsCategoryRequest, Result<AddAssetsCategoryResponse>>
    {
        private readonly ApplicationDbContext _context = context;

        public async Task<Result<AddAssetsCategoryResponse>> Handle(AddAssetsCategoryRequest request, CancellationToken cancellationToken)
        {
            var normalizedName = request.Name.Trim();

            var nameExists = await _context.DesignAssetCategories
               .AnyAsync(c => c.Name == normalizedName, cancellationToken);

            if (nameExists)
            {
                return Result.Failure<AddAssetsCategoryResponse>(AssetsCategoryErrors.CategoryAlreadyExists);
            }
            var category = new DesignAssetCategory
            {
                Name = normalizedName
            };

            _context.DesignAssetCategories.Add(category);

            await _context.SaveChangesAsync(cancellationToken);


            return Result.Success(new AddAssetsCategoryResponse(category.Id));
        }
    }
}
