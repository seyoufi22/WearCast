namespace WearCast.Api.Features.DesignedProductManagement.CustomerDesigns.AddCustomerDesign
{
    public class AddCustomerDesignHandler(
        ApplicationDbContext context,
        IHttpContextAccessor httpContextAccessor,
        ImageService imageService,
        ILogger<AddCustomerDesignHandler> logger
        ) : IRequestHandler<AddCustomerDesignRequest, Result<CustomerDesignResponse>>
    {
        private readonly ApplicationDbContext _context = context;
        private readonly IHttpContextAccessor _httpContextAccessor = httpContextAccessor;
        private readonly ImageService _imageService = imageService;
        private readonly ILogger<AddCustomerDesignHandler> _logger = logger;

        public async Task<Result<CustomerDesignResponse>> Handle(AddCustomerDesignRequest request, CancellationToken cancellationToken)
        {
            var user = _httpContextAccessor.HttpContext!.User;

            var customerId = user.GetCustomerId();
            if (customerId == null)
            {
                return Result.Failure<CustomerDesignResponse>(AuthErrors.Forbidden);
            }

            var productData = await _context.DesignedProductColors
                .Where(c =>
                    c.Id == request.ProductColorId &&
                    c.DesignedProductId == request.ProductId)
                .Select(c => new
                {
                    TemplatePrice = c.DesignedProduct.Price
                })
                .FirstOrDefaultAsync(cancellationToken);

            if (productData == null)
            {
                return Result.Failure<CustomerDesignResponse>(new Error("Design.InvalidColor", "The selected color does not exist for this product.", 400));
            }

            var uploadedUrls = new List<string>();

            try
            {
                string? frontUrl = null, backUrl = null, rightUrl = null, leftUrl = null;

                if (request.FrontImage != null)
                {
                    frontUrl = await _imageService.UploadAsync(request.FrontImage);
                    if (!string.IsNullOrEmpty(frontUrl)) uploadedUrls.Add(frontUrl);
                }

                if (request.BackImage != null)
                {
                    backUrl = await _imageService.UploadAsync(request.BackImage);
                    if (!string.IsNullOrEmpty(backUrl)) uploadedUrls.Add(backUrl);
                }

                if (request.RightImage != null)
                {
                    rightUrl = await _imageService.UploadAsync(request.RightImage);
                    if (!string.IsNullOrEmpty(rightUrl)) uploadedUrls.Add(rightUrl);
                }

                if (request.LeftImage != null)
                {
                    leftUrl = await _imageService.UploadAsync(request.LeftImage);
                    if (!string.IsNullOrEmpty(leftUrl)) uploadedUrls.Add(leftUrl);
                }

                var newDesign = new CustomerDesign
                {
                    CustomerId = customerId.Value,
                    DesignedProductId = request.ProductId,
                    DesignedProductColorId = request.ProductColorId,
                    ViewDesignsJson = request.ViewDesignsJson,
                    FrontImageUrl = frontUrl,
                    BackImageUrl = backUrl,
                    RightImageUrl = rightUrl,
                    LeftImageUrl = leftUrl,
                    AssetCount = request.AssetCount
                };

                decimal fixedAssetPrice = 5.0m;

                newDesign.CalculateAndSetTotalPrice(productData.TemplatePrice, fixedAssetPrice);

                _context.CustomerDesigns.Add(newDesign);
                await _context.SaveChangesAsync(cancellationToken);

                return Result.Success(new CustomerDesignResponse(newDesign.Id));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to create customer design for CustomerId: {CustomerId}", customerId);

                foreach (var url in uploadedUrls)
                {
                    await _imageService.DeleteAsync(url);
                }

                return Result.Failure<CustomerDesignResponse>(new("CustomerDesign.CreationFailed", "An error occurred while saving your design. Please try again.", 500));
            }
        }
    }
}