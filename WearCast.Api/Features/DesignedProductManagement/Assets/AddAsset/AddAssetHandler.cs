using WearCast.Api.Features.DesignedProductManagement.AssetsCategories;

namespace WearCast.Api.Features.DesignedProductManagement.Assets.AddAsset
{
    public class AddAssetHandler(
        ApplicationDbContext context,
        ImageService imageService) : IRequestHandler<AddAssetRequest, Result<AddAssetResponse>>
    {
        private readonly ApplicationDbContext _context = context;
        private readonly ImageService _imageService = imageService;

        public async Task<Result<AddAssetResponse>> Handle(AddAssetRequest request, CancellationToken cancellationToken)
        {
            var categoryExists = await _context.DesignAssetCategories
                .AnyAsync(c => c.Id == request.CategoryId, cancellationToken);

            if (!categoryExists)
            {
                return Result.Failure<AddAssetResponse>(AssetsCategoryErrors.CategoryNotFound);
            }

            var imageUrl = await _imageService.UploadAsync(request.Image);

            var asset = new DesignAsset
            {
                Name = request.Name.Trim(),
                ImageUrl = imageUrl,
                WidthPx = request.WidthPx,
                HeightPx = request.HeightPx,
                DesignAssetCategoryId = request.CategoryId
            };

            _context.DesignAssets.Add(asset);
            await _context.SaveChangesAsync(cancellationToken);

            return Result.Success(new AddAssetResponse(asset.Id));
        }
    }
}
