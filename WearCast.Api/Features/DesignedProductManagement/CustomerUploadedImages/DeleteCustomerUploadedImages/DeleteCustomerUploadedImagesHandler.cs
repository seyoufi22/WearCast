namespace WearCast.Api.Features.DesignedProductManagement.CustomerUploadedImages.DeleteCustomerUploadedImages
{
    public class DeleteCustomerUploadedImagesHandler(
        ApplicationDbContext context,
        IHttpContextAccessor httpContextAccessor,
        ImageService imageService
        ) : IRequestHandler<DeleteCustomerUploadedImagesRequest, Result>
    {
        private readonly ApplicationDbContext _context = context;
        private readonly IHttpContextAccessor _httpContextAccessor = httpContextAccessor;
        private readonly ImageService _imageService = imageService;

        public async Task<Result> Handle(DeleteCustomerUploadedImagesRequest request, CancellationToken cancellationToken)
        {
            var user = _httpContextAccessor.HttpContext!.User;

            var customerId = user.GetCustomerId();
            if (customerId == null)
            {
                return Result.Failure(AuthErrors.Forbidden);
            }

            var customerImage = await _context.CustomerUploadedImages
                .FirstOrDefaultAsync(i =>
                    i.Id == request.Id &&
                    i.CustomerId == customerId.Value,
                cancellationToken);

            if (customerImage == null)
            {
                return Result.Failure(CustomerUploadedImageErrors.ImageNotFound);
            }

            // await _imageService.DeleteAsync(customerImage.ImageUrl);

            customerImage.IsDeleted = true;

            await _context.SaveChangesAsync(cancellationToken);

            return Result.Success();
        }
    }
}
