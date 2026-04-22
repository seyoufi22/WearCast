namespace WearCast.Api.Features.DesignedProductManagement.FactoryProductColors.UpdateFactoryProductColorMainImage
{
    public class UpdateFactoryProductColorMainImageHandler(
                ApplicationDbContext context,
                IHttpContextAccessor httpContextAccessor,
                ImageService imageService,
                ILogger<UpdateFactoryProductColorMainImageHandler> logger
        ) : IRequestHandler<UpdateFactoryProductColorMainImageRequest, Result>
    {
        private readonly ApplicationDbContext _context = context;
        private readonly IHttpContextAccessor _httpContextAccessor = httpContextAccessor;
        private readonly ImageService _imageService = imageService;
        private readonly ILogger<UpdateFactoryProductColorMainImageHandler> _logger = logger;

        public async Task<Result> Handle(UpdateFactoryProductColorMainImageRequest request, CancellationToken cancellationToken)
        {
            var user = _httpContextAccessor.HttpContext!.User;

            var queryResult = await _context.DesignedProductColors
                .Where(x => x.Id == request.ColorId && x.DesignedProductId == request.ProductId)
                .Select(x => new
                {
                    Color = x,
                    FactoryId = x.DesignedProduct.FactoryId
                })
                .FirstOrDefaultAsync(cancellationToken);

            if (queryResult == null)
            {
                return Result.Failure(FactoryProductColorErrors.ColorNotFound);
            }

            var color = queryResult.Color;
            var productFactoryId = queryResult.FactoryId;

            if (user.IsSuperAdmin())
            {
                // No Action 
            }
            else if (user.IsFactoryManager())
            {
                var factoryIdFromToken = user.GetFactoryId();

                if (factoryIdFromToken == null)
                {
                    return Result.Failure(AuthErrors.NoAssociatedFactory);
                }

                if (productFactoryId != factoryIdFromToken.Value)
                {
                    return Result.Failure(AuthErrors.Forbidden);
                }
            }


            try
            {
                var oldImageUrl = color.MainImageUrl;

                var newImageUrl = await _imageService.UploadAsync(request.Image);

                if (newImageUrl == null)
                {
                    return Result.Failure(ImageErrors.UploadFailed);
                }

                color.MainImageUrl = newImageUrl;
                await _context.SaveChangesAsync(cancellationToken);


                await _imageService.DeleteAsync(oldImageUrl);


                return Result.Success();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to update profile image for productColor {targetCustomerId}", request.ColorId);

                return Result.Failure(ImageErrors.UpdateFailed);
            }
        }
    }
}
