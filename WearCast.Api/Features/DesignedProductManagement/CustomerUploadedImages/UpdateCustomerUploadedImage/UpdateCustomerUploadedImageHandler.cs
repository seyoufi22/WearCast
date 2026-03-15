using WearCast.Api.Features.DesignedProductManagement.CustomerUploadedImages.AddCustomerUploadedImage;

namespace WearCast.Api.Features.DesignedProductManagement.CustomerUploadedImages.UpdateCustomerUploadedImage
{
    public class UpdateCustomerUploadedImageHandler(
        ApplicationDbContext context,
        IHttpContextAccessor httpContextAccessor,
        ImageService imageService
        ) : IRequestHandler<UpdateCustomerUploadedImageRequest, Result<CustomerUploadedImageResponse>>
    {
        private readonly ApplicationDbContext _context = context;
        private readonly IHttpContextAccessor _httpContextAccessor = httpContextAccessor;
        private readonly ImageService _imageService = imageService;

        public async Task<Result<CustomerUploadedImageResponse>> Handle(UpdateCustomerUploadedImageRequest request, CancellationToken cancellationToken)
        {
            var user = _httpContextAccessor.HttpContext!.User;

            var customerId = user.GetCustomerId();
            if (customerId == null)
            {
                return Result.Failure<CustomerUploadedImageResponse>(AuthErrors.Forbidden);
            }

            var customerImage = await _context.CustomerUploadedImages
                .FirstOrDefaultAsync(i =>
                    i.Id == request.Id &&
                    i.CustomerId == customerId.Value,
                cancellationToken);

            if (customerImage == null)
            {
                return Result.Failure<CustomerUploadedImageResponse>(CustomerUploadedImageErrors.ImageNotFound);
            }

            var newImageUrl = await _imageService.UploadAsync(request.Image);
            if (string.IsNullOrEmpty(newImageUrl))
            {
                return Result.Failure<CustomerUploadedImageResponse>(new Error("Image.UploadFailed", "Failed to upload the new image.", 500));
            }

            // await _imageService.DeleteAsync(customerImage.ImageUrl); مش هعلم اذ ديليتد لان ممسحتش الرو

            customerImage.ImageUrl = newImageUrl;
            await _context.SaveChangesAsync(cancellationToken);

            return Result.Success(new CustomerUploadedImageResponse(customerImage.Id));
        }
    }
}
