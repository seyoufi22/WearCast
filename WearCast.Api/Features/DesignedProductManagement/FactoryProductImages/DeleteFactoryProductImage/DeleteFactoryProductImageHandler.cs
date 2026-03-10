namespace WearCast.Api.Features.DesignedProductManagement.FactoryProductImages.DeleteFactoryProductImage
{
    public class DeleteFactoryProductImageHandler(
        ApplicationDbContext context,
        ImageService imageService) : IRequestHandler<DeleteFactoryProductImageRequest, Result>
    {
        private readonly ApplicationDbContext _context = context;
        private readonly ImageService _imageService = imageService;

        public async Task<Result> Handle(DeleteFactoryProductImageRequest request, CancellationToken cancellationToken)
        {
            var imageEntity = await _context.DesignedProductImages
                .FirstOrDefaultAsync(i =>
                    i.Id == request.ImageId &&
                    i.DesignedProductColor.Slug == request.ColorSlug,
                cancellationToken);

            if (imageEntity == null)
            {
                return Result.Failure(FactoryProductImageErrors.ImageNotFound);
            }

            await _imageService.DeleteAsync(imageEntity.ImageUrl);

            imageEntity.IsDeleted = true;

            await _context.SaveChangesAsync(cancellationToken);

            return Result.Success();
        }
    }
}
