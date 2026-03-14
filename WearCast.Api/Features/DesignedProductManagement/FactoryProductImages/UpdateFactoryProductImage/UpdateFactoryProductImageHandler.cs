namespace WearCast.Api.Features.DesignedProductManagement.FactoryProductImages.UpdateFactoryProductImage
{
    public class UpdateFactoryProductImageHandler(
        ApplicationDbContext context,
        ImageService imageService) : IRequestHandler<UpdateFactoryProductImageRequest, Result>
    {
        private readonly ApplicationDbContext _context = context;
        private readonly ImageService _imageService = imageService;

        public async Task<Result> Handle(UpdateFactoryProductImageRequest request, CancellationToken cancellationToken)
        {
            var imageEntity = await _context.DesignedProductImages
                .FirstOrDefaultAsync(i =>
                    i.Id == request.ImageId &&
                    i.DesignedProductColor.Slug == request.ColorSlug,
                cancellationToken);

            if (imageEntity == null)
                return Result.Failure(FactoryProductImageErrors.ImageNotFound);


            if (imageEntity.ViewSide != request.ViewSide)
            {
                var sideAlreadyExists = await _context.DesignedProductImages
                    .AnyAsync(i =>
                        i.DesignedProductColorId == imageEntity.DesignedProductColorId &&
                        i.ViewSide == request.ViewSide,
                    cancellationToken);

                if (sideAlreadyExists)
                {
                    return Result.Failure(FactoryProductImageErrors.ImageSideAlreadyExists);
                }

                imageEntity.ViewSide = request.ViewSide;
            }

            if (request.Image != null)
            {
                await _imageService.DeleteAsync(imageEntity.ImageUrl);

                imageEntity.ImageUrl = await _imageService.UploadAsync(request.Image);
            }


            await _context.SaveChangesAsync(cancellationToken);

            return Result.Success();
        }
    }
}
