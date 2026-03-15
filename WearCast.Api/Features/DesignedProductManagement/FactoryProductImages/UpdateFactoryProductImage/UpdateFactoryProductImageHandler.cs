namespace WearCast.Api.Features.DesignedProductManagement.FactoryProductImages.UpdateFactoryProductImage
{
    public class UpdateFactoryProductImageHandler(
        ApplicationDbContext context,
        ImageService imageService,
        IHttpContextAccessor httpContextAccessor
        ) : IRequestHandler<UpdateFactoryProductImageRequest, Result<FactoryProductImagesResponse>>
    {
        private readonly ApplicationDbContext _context = context;
        private readonly ImageService _imageService = imageService;
        private readonly IHttpContextAccessor _httpContextAccessor = httpContextAccessor;

        public async Task<Result<FactoryProductImagesResponse>> Handle(UpdateFactoryProductImageRequest request, CancellationToken cancellationToken)
        {
            var user = _httpContextAccessor.HttpContext!.User;

            var isSuperAdmin = user.IsSuperAdmin();
            var userFactoryId = user.GetFactoryId();

            var data = await _context.DesignedProductImages
                .Where(i => i.Id == request.ImageId)
                .Select(i => new
                {
                    Entity = i,
                    FactoryId = i.DesignedProductColor.DesignedProduct.FactoryId,
                    ColorId = i.DesignedProductColorId
                })
                .FirstOrDefaultAsync(cancellationToken);

            if (data == null)
                return Result.Failure<FactoryProductImagesResponse>(FactoryProductImageErrors.ImageNotFound);

            if (!isSuperAdmin && data.FactoryId != userFactoryId)
                return Result.Failure<FactoryProductImagesResponse>(AuthErrors.Forbidden);

            if (data.Entity.ViewSide != request.ViewSide)
            {
                var sideAlreadyExists = await _context.DesignedProductImages
                    .AnyAsync(i =>
                        i.DesignedProductColorId == data.ColorId &&
                        i.ViewSide == request.ViewSide,
                    cancellationToken);

                if (sideAlreadyExists)
                    return Result.Failure<FactoryProductImagesResponse>(FactoryProductImageErrors.ImageSideAlreadyExists);

                data.Entity.ViewSide = request.ViewSide;
            }

            if (request.Image != null)
            {
                var newImageUrl = await _imageService.UploadAsync(request.Image);
                if (string.IsNullOrEmpty(newImageUrl))
                    return Result.Failure<FactoryProductImagesResponse>(new Error("Image.UploadFailed", "Failed to upload image", 500));

                await _imageService.DeleteAsync(data.Entity.ImageUrl);
                data.Entity.ImageUrl = newImageUrl;
            }

            await _context.SaveChangesAsync(cancellationToken);

            return Result.Success(new FactoryProductImagesResponse(data.Entity.Id));
        }
    }
}