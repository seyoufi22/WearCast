namespace WearCast.Api.Features.DesignedProductManagement.FactoryProductImages.DeleteFactoryProductImage
{
    public class DeleteFactoryProductImageHandler(
        ApplicationDbContext context,
        ImageService imageService,
        IHttpContextAccessor httpContextAccessor
        ) : IRequestHandler<DeleteFactoryProductImageRequest, Result>
    {
        private readonly ApplicationDbContext _context = context;
        private readonly ImageService _imageService = imageService;
        private readonly IHttpContextAccessor _httpContextAccessor = httpContextAccessor;

        public async Task<Result> Handle(DeleteFactoryProductImageRequest request, CancellationToken cancellationToken)
        {
            var user = _httpContextAccessor.HttpContext!.User;

            var isAdmin = user.IsSuperAdmin() || user.IsCatalogAdmin();
            var userFactoryId = user.GetFactoryId();

            var imageData = await _context.DesignedProductImages
                .Where(i => i.Id == request.ImageId)
                .Select(i => new
                {
                    Entity = i,
                    FactoryId = i.DesignedProductColor.DesignedProduct.FactoryId
                })
                .FirstOrDefaultAsync(cancellationToken);

            if (imageData == null)
            {
                return Result.Failure(FactoryProductImageErrors.ImageNotFound);
            }

            if (!isAdmin && imageData.FactoryId != userFactoryId)
            {
                return Result.Failure(AuthErrors.Forbidden);
            }

            await _imageService.DeleteAsync(imageData.Entity.ImageUrl);

            imageData.Entity.IsDeleted = true;

            await _context.SaveChangesAsync(cancellationToken);

            return Result.Success();
        }
    }
}