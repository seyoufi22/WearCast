namespace WearCast.Api.Features.DesignedProductManagement.CustomerUploadedImages.AddCustomerUploadedImage
{
    public class AddCustomerUploadedImageHandler(
        ApplicationDbContext context,
        ImageService imageService,
        IHttpContextAccessor httpContextAccessor
        ) : IRequestHandler<AddCustomerUploadedImageRequest, Result<CustomerUploadedImageResponse>>
    {
        private readonly ApplicationDbContext _context = context;
        private readonly ImageService _imageService = imageService;
        private readonly IHttpContextAccessor _httpContextAccessor = httpContextAccessor;

        public async Task<Result<CustomerUploadedImageResponse>> Handle(AddCustomerUploadedImageRequest request, CancellationToken cancellationToken)
        {
            var user = _httpContextAccessor.HttpContext!.User;

            var customerId = user.GetCustomerId();

            if (customerId == null)
            {
                return Result.Failure<CustomerUploadedImageResponse>(AuthErrors.Forbidden);
            }

            var imageUrl = await _imageService.UploadAsync(request.Image);

            if (string.IsNullOrEmpty(imageUrl))
            {
                return Result.Failure<CustomerUploadedImageResponse>(new Error("Image.UploadFailed", "Failed to upload your image. Please try again.", 500));
            }

            var customerImage = new CustomerUploadedImage
            {
                CustomerId = customerId.Value,
                ImageUrl = imageUrl
            };

            _context.CustomerUploadedImages.Add(customerImage);
            await _context.SaveChangesAsync(cancellationToken);

            return Result.Success(new CustomerUploadedImageResponse(customerImage.Id));
        }
    }
}
