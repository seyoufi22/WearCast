namespace WearCast.Api.Features.DesignedProductManagement.Assets.UpdateAsset
{
    public class UpdateAssetHandler(
        ApplicationDbContext context,
        IHttpContextAccessor httpContextAccessor,
        ImageService imageService) : IRequestHandler<UpdateAssetRequest, Result>
    {
        private readonly ApplicationDbContext _context = context;
        private readonly IHttpContextAccessor _httpContextAccessor = httpContextAccessor;
        private readonly ImageService _imageService = imageService;

        public async Task<Result> Handle(UpdateAssetRequest request, CancellationToken cancellationToken)
        {
            var asset = await _context.DesignAssets
                .FirstOrDefaultAsync(x => x.Id == request.Id, cancellationToken);

            if (asset == null)
            {
                return Result.Failure(AssetErrors.AssetNotFound);
            }

            if (asset.DesignAssetCategoryId != request.CategoryId)
            {
                var categoryIsExists = await _context.DesignAssetCategories
                    .AnyAsync(c => c.Id == request.CategoryId, cancellationToken);

                if (!categoryIsExists)
                {
                    return Result.Failure(new Error("Category.NotFound", "The specified category does not exist.", StatusCodes.Status400BadRequest));
                }
                asset.DesignAssetCategoryId = request.CategoryId;
            }

            asset.Name = request.Name;
            asset.WidthPx = request.WidthPx;
            asset.HeightPx = request.HeightPx;

            if (request.Image != null)
            {
                var newImageUrl = await _imageService.UploadAsync(request.Image);
                if (string.IsNullOrEmpty(newImageUrl))
                {
                    return Result.Failure(new Error("Image.UploadFailed", "Failed to upload the new asset image.", 500));
                }

                asset.ImageUrl = newImageUrl;
            }
            await _context.SaveChangesAsync(cancellationToken);

            return Result.Success();
        }
    }
}
