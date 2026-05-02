namespace WearCast.Api.Features.Customers.CutomerImage.UpdateCusomerImage
{
    public class UpdateCustomerImageHandler(
        ApplicationDbContext context,
        IHttpContextAccessor httpContextAccessor,
        ImageService imageService,
        ILogger<UpdateCustomerImageHandler> logger
    ) : IRequestHandler<UpdateCustomerImageRequest, Result>
    {
        private readonly ApplicationDbContext _context = context;
        private readonly IHttpContextAccessor _httpContextAccessor = httpContextAccessor;
        private readonly ImageService _imageService = imageService;
        private readonly ILogger<UpdateCustomerImageHandler> _logger = logger;

        public async Task<Result> Handle(UpdateCustomerImageRequest request, CancellationToken cancellationToken)
        {
            var user = _httpContextAccessor.HttpContext!.User;

            int targetCustomerId;

            if (user.IsSuperAdmin() || user.IsCustomerServiceAdmin())
            {
                if (!request.ProvidedCustomerId.HasValue)
                {
                    return Result.Failure(new Error("Validation.MissingId", "SuperAdmin must provide a target CustomerId to delete.", StatusCodes.Status400BadRequest));
                }

                targetCustomerId = request.ProvidedCustomerId.Value;
            }
            else
            {
                targetCustomerId = user.GetCustomerId()!.Value;
            }

            var customer = await _context.Customers
                .FirstOrDefaultAsync(x => x.Id == targetCustomerId, cancellationToken);

            if (customer == null)
            {
                return Result.Failure(CustomerErrors.CustomerNotFound);
            }

            try
            {
                var oldImageUrl = customer.ProfileImageUrl;

                var newImageUrl = await _imageService.UploadAsync(request.NewImage);

                if (newImageUrl == null)
                {
                    return Result.Failure(ImageErrors.UploadFailed);
                }

                customer.ProfileImageUrl = newImageUrl;
                await _context.SaveChangesAsync(cancellationToken);


                await _imageService.DeleteAsync(oldImageUrl);


                return Result.Success();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to update profile image for customer {targetCustomerId}", targetCustomerId);

                return Result.Failure(ImageErrors.UpdateFailed);
            }
        }
    }
}

