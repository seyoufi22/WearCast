using WearCast.Api.Features.DesignedProductManagement.FactoryProductColors;

namespace WearCast.Api.Features.DesignedProductManagement.FactoryProductImages.UpsertFactoryProductImage
{
    public class UpsertFactoryProductImageHandler(
        ApplicationDbContext context,
        IHttpContextAccessor httpContextAccessor,
        ImageService imageService,
        ILogger<UpsertFactoryProductImageHandler> logger
        ) : IRequestHandler<UpsertFactoryProductImageRequest, Result<FactoryProductImagesResponse>>
    {
        public async Task<Result<FactoryProductImagesResponse>> Handle(UpsertFactoryProductImageRequest request, CancellationToken cancellationToken)
        {
            var user = httpContextAccessor.HttpContext!.User;

            var queryResult = await context.DesignedProductColors
                .Where(c => c.Id == request.ColorId)
                .Select(c => new
                {
                    ColorId = c.Id,
                    FactoryId = c.DesignedProduct.FactoryId,

                    ExistingImageId = c.Images
                        .Where(img => img.ViewSide == request.ViewSide)
                        .Select(img => (int?)img.Id)
                        .FirstOrDefault()
                })
                .FirstOrDefaultAsync(cancellationToken);

            if (queryResult == null)
            {
                return Result.Failure<FactoryProductImagesResponse>(FactoryProductColorErrors.ColorNotFound);
            }

            if (user.IsSuperAdmin() || user.IsCatalogAdmin())
            {
                // No Action
            }
            else if (user.IsFactoryManager())
            {
                var factoryIdFromToken = user.GetFactoryId();
                if (factoryIdFromToken == null) return Result.Failure<FactoryProductImagesResponse>(AuthErrors.NoAssociatedFactory);

                if (queryResult.FactoryId != factoryIdFromToken.Value)
                    return Result.Failure<FactoryProductImagesResponse>(AuthErrors.Forbidden);
            }
            else
            {
                return Result.Failure<FactoryProductImagesResponse>(AuthErrors.Forbidden);
            }

            var newImageUrl = await imageService.UploadAsync(request.NewImage);
            if (string.IsNullOrEmpty(newImageUrl))
            {
                return Result.Failure<FactoryProductImagesResponse>(new("Image.UploadFailed", "Failed to upload the new image.", 500));
            }

            try
            {
                int finalImageId;

                if (queryResult.ExistingImageId.HasValue)
                {
                    await context.DesignedProductImages
                        .Where(img => img.Id == queryResult.ExistingImageId.Value)
                        .ExecuteUpdateAsync(s => s.SetProperty(p => p.ImageUrl, newImageUrl), cancellationToken);

                    finalImageId = queryResult.ExistingImageId.Value;
                }
                else
                {
                    var newImage = new DesignedProductImage
                    {
                        DesignedProductColorId = request.ColorId,
                        ViewSide = request.ViewSide,
                        ImageUrl = newImageUrl
                    };
                    context.DesignedProductImages.Add(newImage);
                    await context.SaveChangesAsync(cancellationToken);

                    finalImageId = newImage.Id;
                }

                return Result.Success(new FactoryProductImagesResponse(finalImageId));
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "DB save failed. Rolling back uploaded image: {Url}", newImageUrl);
                await imageService.DeleteAsync(newImageUrl);

                return Result.Failure<FactoryProductImagesResponse>(new("FactoryProductColor.UpdateFailed", "Error saving image to database.", 500));
            }
        }
    }
}