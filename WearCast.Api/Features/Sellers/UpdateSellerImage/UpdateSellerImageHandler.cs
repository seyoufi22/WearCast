using WearCast.Api.Features.Drivers.UpdateDriverImage;

namespace WearCast.Api.Features.Sellers.UpdateSellerImage
{
    public class UpdateSellerImageHandler(
        ApplicationDbContext context,
        IHttpContextAccessor httpContextAccessor,
        ImageService imageService,
        ILogger<UpdateDriverImageHandler> logger
        ) : IRequestHandler<UpdateSellerImageRequest, Result>
    {
        private readonly ApplicationDbContext _context = context;
        private readonly IHttpContextAccessor _httpContextAccessor = httpContextAccessor;
        private readonly ImageService _imageService = imageService;
        private readonly ILogger<UpdateDriverImageHandler> _logger = logger;
        public async Task<Result> Handle(UpdateSellerImageRequest request, CancellationToken cancellationToken)
        {
            var user = _httpContextAccessor.HttpContext!.User;

            int targetSellerId;

            if (user.IsSuperAdmin())
            {
                if (!request.ProvidedSellerId.HasValue)
                {
                    return Result.Failure(new Error("Validation.MissingId", "SuperAdmin must provide a target ProviderId to delete.", StatusCodes.Status400BadRequest));
                }

                targetSellerId = request.ProvidedSellerId.Value;
            }
            else
            {
                targetSellerId = user.GetSellerId()!.Value;
            }

            var seller = await _context.Sellers
                .FirstOrDefaultAsync(x => x.Id == targetSellerId, cancellationToken);

            if (seller == null)
            {
                return Result.Failure(SellerErrors.SellerNotFound);
            }

            var oldLogoUrl = seller.LogoUrl;

            var newLogoUrl = await _imageService.UploadAsync(request.NewLogo);
            if (newLogoUrl == null)
            {
                return Result.Failure(ImageErrors.UploadFailed);
            }

            seller.LogoUrl = newLogoUrl;

            try
            {
                await _context.SaveChangesAsync(cancellationToken);
            }
            catch
            {
                await _imageService.DeleteAsync(newLogoUrl);
                return Result.Failure(new Error("Database.SaveFailed", "Failed to save the new Logo to the database.", StatusCodes.Status500InternalServerError));
            }

            await _imageService.DeleteAsync(oldLogoUrl);

            return Result.Success();
        }
    }
}
