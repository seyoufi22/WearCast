using WearCast.Api.Features.DesignedProductManagement.FactoryProductColors;
// متنساش مسار الـ Errors بتاعتك

namespace WearCast.Api.Features.DesignedProductManagement.FactoryProductImages.AddFactoryProductImage
{
    public class AddFactoryProductImageHandler(
        ApplicationDbContext context,
        ImageService imageService,
        IHttpContextAccessor httpContextAccessor
        ) : IRequestHandler<AddFactoryProductImageRequest, Result>
    {
        private readonly ApplicationDbContext _context = context;
        private readonly ImageService _imageService = imageService;
        private readonly IHttpContextAccessor _httpContextAccessor = httpContextAccessor;

        public async Task<Result> Handle(AddFactoryProductImageRequest request, CancellationToken cancellationToken)
        {
            var user = _httpContextAccessor.HttpContext!.User;

            var isSuperAdmin = user.IsSuperAdmin();
            var userFactoryId = user.GetFactoryId();

            var colorData = await _context.DesignedProductColors
                .Where(c => c.Id == request.ColorId)
                .Select(c => new
                {
                    FactoryId = c.DesignedProduct.FactoryId,

                    SideAlreadyExists = c.Images.Any(i => i.ViewSide == request.ViewSide)
                })
                .FirstOrDefaultAsync(cancellationToken);

            if (colorData == null)
            {
                return Result.Failure(FactoryProductColorErrors.ColorNotFound);
            }

            if (!isSuperAdmin && colorData.FactoryId != userFactoryId)
            {
                return Result.Failure(AuthErrors.Forbidden);
            }

            if (colorData.SideAlreadyExists)
            {
                return Result.Failure(FactoryProductImageErrors.ImageSideAlreadyExists);
            }

            var imageUrl = await _imageService.UploadAsync(request.Image);

            if (string.IsNullOrEmpty(imageUrl))
            {
                return Result.Failure(new Error("Image.UploadFailed", "Failed to upload the image.", StatusCodes.Status500InternalServerError));
            }

            var productImage = new DesignedProductImage
            {
                DesignedProductColorId = request.ColorId,
                ImageUrl = imageUrl,
                ViewSide = request.ViewSide
            };

            _context.DesignedProductImages.Add(productImage);
            await _context.SaveChangesAsync(cancellationToken);

            return Result.Success();
        }
    }
}