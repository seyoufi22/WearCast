namespace WearCast.Api.Features.DesignedProductManagement.CustomerDesigns.DeleteCustomerDesignImage
{
    public class DeleteCustomerDesignImageHandler(
        ApplicationDbContext context,
        ImageService imageService,
        IHttpContextAccessor httpContextAccessor,
        ILogger<DeleteCustomerDesignImageHandler> logger
        ) : IRequestHandler<DeleteCustomerDesignImageRequest, Result>
    {
        private readonly ApplicationDbContext _context = context;
        private readonly ImageService _imageService = imageService;
        private readonly IHttpContextAccessor _httpContextAccessor = httpContextAccessor;
        private readonly ILogger<DeleteCustomerDesignImageHandler> _logger = logger;
        public async Task<Result> Handle(DeleteCustomerDesignImageRequest request, CancellationToken cancellationToken)
        {
            var customerId = _httpContextAccessor.HttpContext?.User?.GetCustomerId();
            if (customerId == null) return Result.Failure(AuthErrors.Forbidden);

            var design = await _context.CustomerDesigns
                .FirstOrDefaultAsync(d =>
                    d.Id == request.Id &&
                    d.CustomerId == customerId.Value,
                cancellationToken);

            if (design == null) return Result.Failure(CustomerDesignErrors.DesignNotFound);

            string? urlToDelete = null;

            switch (request.Side)
            {
                case ViewSide.Front:
                    urlToDelete = design.FrontImageUrl;
                    design.FrontImageUrl = null;
                    break;
                case ViewSide.Back:
                    urlToDelete = design.BackImageUrl;
                    design.BackImageUrl = null;
                    break;
                case ViewSide.Right:
                    urlToDelete = design.RightImageUrl;
                    design.RightImageUrl = null;
                    break;
                case ViewSide.Left:
                    urlToDelete = design.LeftImageUrl;
                    design.LeftImageUrl = null;
                    break;
            }

            if (string.IsNullOrEmpty(urlToDelete))
            {
                return Result.Success();
            }

            try
            {
                await _context.SaveChangesAsync(cancellationToken);
                await _imageService.DeleteAsync(urlToDelete);

                return Result.Success();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to delete {Side} image for design {DesignId}", request.Side.ToString(), request.Id);
                return Result.Failure(new("CustomerDesign.DeletionFailed", "An unexpected error occurred while deleting your design image. Please try again.", StatusCodes.Status500InternalServerError));
            }
        }
    }
}
