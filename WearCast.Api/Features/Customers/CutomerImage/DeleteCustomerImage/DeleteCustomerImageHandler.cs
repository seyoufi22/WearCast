

namespace WearCast.Api.Features.Customers.CutomerImage.DeleteCustomerImage
{
    public class DeleteCustomerImageHandler(
        ApplicationDbContext context,
        IHttpContextAccessor httpContextAccessor,
        ImageService imageService
    ) : IRequestHandler<DeleteCustomerImageRequest, Result>
    {
        private readonly ApplicationDbContext _context = context;
        private readonly IHttpContextAccessor _httpContextAccessor = httpContextAccessor;
        private readonly ImageService _imageService = imageService;

        public async Task<Result> Handle(DeleteCustomerImageRequest request, CancellationToken cancellationToken)
        {
            var user = _httpContextAccessor.HttpContext!.User;

            int targetCustomerId;

            if (user.IsSuperAdmin())
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

            if (string.IsNullOrEmpty(customer.ProfileImageUrl))
            {
                return Result.Success();
            }

            await _imageService.DeleteAsync(customer.ProfileImageUrl);

            customer.ProfileImageUrl = null;
            await _context.SaveChangesAsync(cancellationToken);

            return Result.Success();
        }
    }
}