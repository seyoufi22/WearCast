using WearCast.Api.Features.DesignedProductManagement.FactoryProductColors;

namespace WearCast.Api.Features.DesignedProductManagement.FactoryProductImages.AddFactoryProductImage
{
    public class AddFactoryProductImageHandler(
        ApplicationDbContext context,
        ImageService imageService) : IRequestHandler<AddFactoryProductImageRequest, Result>
    {
        private readonly ApplicationDbContext _context = context;
        private readonly ImageService _imageService = imageService;

        public async Task<Result> Handle(AddFactoryProductImageRequest request, CancellationToken cancellationToken)
        {
            var colorId = await _context.DesignedProductColors
                .Where(c => c.Slug == request.ColorSlug)
                .Select(c => (int?)c.Id)
                .FirstOrDefaultAsync(cancellationToken);

            if (colorId == null)
                return Result.Failure(FactoryProductColorErrors.ColorNotFound);

            var sideAlreadyExists = await _context.Set<DesignedProductImage>()
                .AnyAsync(i =>
                    i.DesignedProductColorId == colorId.Value &&
                    i.ViewSide == request.ViewSide,
                cancellationToken);

            if (sideAlreadyExists)
                return Result.Failure(FactoryProductImageErrors.ImageSideAlreadyExists);


            var productImage = new DesignedProductImage
            {
                DesignedProductColorId = colorId.Value,
                ImageUrl = await _imageService.UploadAsync(request.Image),
                ViewSide = request.ViewSide
            };

            _context.DesignedProductImages.Add(productImage);
            await _context.SaveChangesAsync(cancellationToken);

            return Result.Success();
        }
    }
}
