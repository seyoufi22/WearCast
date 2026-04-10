namespace WearCast.Api.Features.DesignedProductManagement.CustomerDesigns.DeleteCustomerDesign
{
    public class DeleteCustomerDesignHandler(
        ApplicationDbContext context,
        ImageService imageService,
        IHttpContextAccessor httpContextAccessor,
        ILogger<DeleteCustomerDesignHandler> logger
        ) : IRequestHandler<DeleteCustomerDesignRequest, Result>
    {
        private readonly ApplicationDbContext _context = context;
        private readonly ImageService _imageService = imageService;
        private readonly IHttpContextAccessor _httpContextAccessor = httpContextAccessor;
        private readonly ILogger<DeleteCustomerDesignHandler> _logger = logger;

        public async Task<Result> Handle(DeleteCustomerDesignRequest request, CancellationToken cancellationToken)
        {
            var customerId = _httpContextAccessor.HttpContext?.User?.GetCustomerId();
            if (customerId == null) return Result.Failure(AuthErrors.Forbidden);

            var design = await _context.CustomerDesigns
                .FirstOrDefaultAsync(d =>
                    d.Id == request.Id &&
                    d.CustomerId == customerId.Value,
                cancellationToken);

            if (design == null) return Result.Failure(CustomerDesignErrors.DesignNotFound);

            var isInCart = await _context.CartItems
                .AnyAsync(c => c.CustomerDesignId == request.Id, cancellationToken);

            if (isInCart) return Result.Failure(CustomerDesignErrors.DesignInCart);


            var urlsToDelete = new List<string>();
            if (!string.IsNullOrEmpty(design.FrontImageUrl)) urlsToDelete.Add(design.FrontImageUrl);
            if (!string.IsNullOrEmpty(design.BackImageUrl)) urlsToDelete.Add(design.BackImageUrl);
            if (!string.IsNullOrEmpty(design.RightImageUrl)) urlsToDelete.Add(design.RightImageUrl);
            if (!string.IsNullOrEmpty(design.LeftImageUrl)) urlsToDelete.Add(design.LeftImageUrl);

            try
            {
                design.FrontImageUrl = null;
                design.BackImageUrl = null;
                design.RightImageUrl = null;
                design.LeftImageUrl = null;

                _context.CustomerDesigns.Remove(design);
                await _context.SaveChangesAsync(cancellationToken);

                foreach (var url in urlsToDelete)
                {
                    await _imageService.DeleteAsync(url);
                }

                return Result.Success();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to delete customer design {DesignId}", request.Id);
                return Result.Failure(new("CustomerDesign.DeletionFailed", "An unexpected error occurred while deleting your design. Please try again.", StatusCodes.Status500InternalServerError));
            }
        }
    }
}