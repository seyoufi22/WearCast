namespace WearCast.Api.Features.Drivers.UpdateDriverImage
{
    public class UpdateDriverImageHandler(
        ApplicationDbContext context,
        IHttpContextAccessor httpContextAccessor,
        ImageService imageService,
        ILogger<UpdateDriverImageHandler> logger
        ) : IRequestHandler<UpdateDriverImageRequest, Result>
    {
        private readonly ApplicationDbContext _context = context;
        private readonly IHttpContextAccessor _httpContextAccessor = httpContextAccessor;
        private readonly ImageService _imageService = imageService;
        private readonly ILogger<UpdateDriverImageHandler> _logger = logger;

        public async Task<Result> Handle(UpdateDriverImageRequest request, CancellationToken cancellationToken)
        {
            var user = _httpContextAccessor.HttpContext!.User;

            int targetDriverId;

            if (user.IsSuperAdmin())
            {
                if (!request.ProvidedDriverId.HasValue)
                {
                    return Result.Failure(new Error("Validation.MissingId", "SuperAdmin must provide a target ProviderId to delete.", StatusCodes.Status400BadRequest));
                }

                targetDriverId = request.ProvidedDriverId.Value;
            }
            else
            {
                targetDriverId = user.GetDriverId()!.Value;
            }

            var driver = await _context.Drivers
                .FirstOrDefaultAsync(x => x.Id == targetDriverId, cancellationToken);

            if (driver == null)
            {
                return Result.Failure(DriverErrors.NotFound);
            }

            var oldImageUrl = driver.ProfileImageUrl;

            var newImageUrl = await _imageService.UploadAsync(request.NewImage);
            if (newImageUrl == null)
            {
                return Result.Failure(ImageErrors.UploadFailed);
            }

            driver.ProfileImageUrl = newImageUrl;

            try
            {
                await _context.SaveChangesAsync(cancellationToken);
            }
            catch
            {
                await _imageService.DeleteAsync(newImageUrl);
                return Result.Failure(new Error("Database.SaveFailed", "Failed to save the new image to the database.", StatusCodes.Status500InternalServerError));
            }

            await _imageService.DeleteAsync(oldImageUrl);

            return Result.Success();

        }
    }
}
