namespace WearCast.Api.Features.DesignedProductManagement.CustomerDesigns.UpdateCustomerDesign
{
    public class UpdateCustomerDesignHandler(
        ApplicationDbContext context,
        ImageService imageService,
        IHttpContextAccessor httpContextAccessor,
        ILogger<UpdateCustomerDesignHandler> logger
        ) : IRequestHandler<UpdateCustomerDesignRequest, Result<CustomerDesignResponse>>
    {
        private readonly ApplicationDbContext _context = context;
        private readonly ImageService _imageService = imageService;
        private readonly IHttpContextAccessor _httpContextAccessor = httpContextAccessor;
        private readonly ILogger<UpdateCustomerDesignHandler> _logger = logger;

        public async Task<Result<CustomerDesignResponse>> Handle(UpdateCustomerDesignRequest request, CancellationToken cancellationToken)
        {
            var customerId = _httpContextAccessor.HttpContext?.User?.GetCustomerId();
            if (customerId == null) return Result.Failure<CustomerDesignResponse>(AuthErrors.Forbidden);

            var queryResult = await _context.CustomerDesigns
                .Where(d => d.Id == request.Id && d.CustomerId == customerId.Value)
                .Select(d => new
                {
                    Design = d,
                    TemplatePrice = d.DesignedProduct.Price
                })
                .FirstOrDefaultAsync(cancellationToken);

            if (queryResult == null)
            {
                return Result.Failure<CustomerDesignResponse>(CustomerDesignErrors.DesignNotFound);
            }

            var design = queryResult.Design;
            var templatePrice = queryResult.TemplatePrice;

            var newUploadedUrls = new List<string>();
            var oldUrlsToDelete = new List<string>();

            try
            {
                if (request.FrontImage != null)
                {
                    var newUrl = await _imageService.UploadAsync(request.FrontImage);
                    if (!string.IsNullOrEmpty(newUrl))
                    {
                        newUploadedUrls.Add(newUrl);
                        if (!string.IsNullOrEmpty(design.FrontImageUrl)) oldUrlsToDelete.Add(design.FrontImageUrl);
                        design.FrontImageUrl = newUrl;
                    }
                }

                if (request.BackImage != null)
                {
                    var newUrl = await _imageService.UploadAsync(request.BackImage);
                    if (!string.IsNullOrEmpty(newUrl))
                    {
                        newUploadedUrls.Add(newUrl);
                        if (!string.IsNullOrEmpty(design.BackImageUrl)) oldUrlsToDelete.Add(design.BackImageUrl);
                        design.BackImageUrl = newUrl;
                    }
                }

                if (request.RightImage != null)
                {
                    var newUrl = await _imageService.UploadAsync(request.RightImage);
                    if (!string.IsNullOrEmpty(newUrl))
                    {
                        newUploadedUrls.Add(newUrl);
                        if (!string.IsNullOrEmpty(design.RightImageUrl)) oldUrlsToDelete.Add(design.RightImageUrl);
                        design.RightImageUrl = newUrl;
                    }
                }

                if (request.LeftImage != null)
                {
                    var newUrl = await _imageService.UploadAsync(request.LeftImage);
                    if (!string.IsNullOrEmpty(newUrl))
                    {
                        newUploadedUrls.Add(newUrl);
                        if (!string.IsNullOrEmpty(design.LeftImageUrl)) oldUrlsToDelete.Add(design.LeftImageUrl);
                        design.LeftImageUrl = newUrl;
                    }
                }

                design.ViewDesignsJson = request.ViewDesignsJson;

                if (design.AssetCount != request.AssetCount)
                {
                    design.AssetCount = request.AssetCount;

                    decimal fixedAssetPrice = 5.0m;

                    design.CalculateAndSetTotalPrice(templatePrice, fixedAssetPrice);
                }

                await _context.SaveChangesAsync(cancellationToken);

                foreach (var oldUrl in oldUrlsToDelete)
                {
                    await _imageService.DeleteAsync(oldUrl);
                }

                return Result.Success(new CustomerDesignResponse(design.Id));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to update customer design {DesignId}", request.Id);

                foreach (var newUrl in newUploadedUrls)
                {
                    await _imageService.DeleteAsync(newUrl);
                }

                return Result.Failure<CustomerDesignResponse>(new("CustomerDesign.UpdateFailed", "An error occurred while saving your design. Please try again.", 500));
            }
        }
    }
}