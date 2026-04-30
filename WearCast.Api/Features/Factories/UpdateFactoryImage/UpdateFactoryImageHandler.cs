namespace WearCast.Api.Features.Factories.UpdateFactoryImage
{
    public class UpdateFactoryImageHandler(
        ApplicationDbContext context,
        IHttpContextAccessor httpContextAccessor,
        ImageService imageService
        ) : IRequestHandler<UpdateFactoryImageRequest, Result>
    {
        private readonly ApplicationDbContext _context = context;
        private readonly IHttpContextAccessor _httpContextAccessor = httpContextAccessor;
        private readonly ImageService _imageService = imageService;

        public async Task<Result> Handle(UpdateFactoryImageRequest request, CancellationToken cancellationToken)
        {
            var user = _httpContextAccessor.HttpContext!.User;

            int targetFactoryId;

            if (user.IsSuperAdmin() || user.IsVendorAdmin())
            {
                if (!request.ProvidedFactoryId.HasValue)
                {
                    return Result.Failure(new Error("Validation.MissingId", "SuperAdmin must provide a target ProviderId to delete.", StatusCodes.Status400BadRequest));
                }

                targetFactoryId = request.ProvidedFactoryId.Value;
            }
            else
            {
                targetFactoryId = user.GetFactoryId()!.Value;
            }

            var factory = await _context.Factories
                .FirstOrDefaultAsync(x => x.Id == targetFactoryId, cancellationToken);

            if (factory == null)
            {
                return Result.Failure(FactoryErrors.FactoryNotFound);
            }

            var oldLogoUrl = factory.LogoUrl;

            var newLogoUrl = await _imageService.UploadAsync(request.NewLogo);
            if (newLogoUrl == null)
            {
                return Result.Failure(ImageErrors.UploadFailed);
            }

            factory.LogoUrl = newLogoUrl;

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