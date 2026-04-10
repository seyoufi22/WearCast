namespace WearCast.Api.Features.DesignedProductManagement.FactoryProductColors.AddFactoryProductColor
{
    public class AddFactoryProductColorHandler(
        ApplicationDbContext context,
        IHttpContextAccessor httpContextAccessor,
        ImageService imageService,
        ILogger<AddFactoryProductColorHandler> logger
        ) : IRequestHandler<AddFactoryProductColorRequest, Result<AddFactoryProductColorResponse>>
    {
        private readonly ApplicationDbContext _context = context;
        private readonly IHttpContextAccessor _httpContextAccessor = httpContextAccessor;
        private readonly ImageService _imageService = imageService;
        private readonly ILogger<AddFactoryProductColorHandler> _logger = logger;

        public async Task<Result<AddFactoryProductColorResponse>> Handle(AddFactoryProductColorRequest request, CancellationToken cancellationToken)
        {
            var user = _httpContextAccessor.HttpContext!.User;


            var factoryId = await _context.DesignedProducts
                 .Where(x => x.Id == request.ProductId)
                 .Select(x => (int?)x.FactoryId)
                 .FirstOrDefaultAsync(cancellationToken);

            if (factoryId == null)
            {
                return Result.Failure<AddFactoryProductColorResponse>(DesignedProductErrors.ProductNotFound);
            }

            if (user.IsSuperAdmin())
            {
                //No Action
            }
            else if (user.IsFactoryManager())
            {
                var factoryIdFromToken = user.GetFactoryId();

                if (factoryIdFromToken == null)
                {
                    return Result.Failure<AddFactoryProductColorResponse>(AuthErrors.NoAssociatedFactory);
                }

                if (factoryId != factoryIdFromToken.Value)
                {
                    return Result.Failure<AddFactoryProductColorResponse>(AuthErrors.Forbidden);
                }
            }
            else
            {
                return Result.Failure<AddFactoryProductColorResponse>(AuthErrors.Forbidden);
            }

            var colorExists = await _context.DesignedProductColors
                .AnyAsync(c => c.DesignedProductId == request.ProductId && c.HexCode == request.HexCode, cancellationToken);

            if (colorExists)
            {
                return Result.Failure<AddFactoryProductColorResponse>(FactoryProductColorErrors.ColorAlreadyExists);
            }

            var imageUrl = await _imageService.UploadAsync(request.Image);
            if (string.IsNullOrEmpty(imageUrl))
            {
                return Result.Failure<AddFactoryProductColorResponse>(ImageErrors.UploadFailed);
            }

            var newProductColor = new DesignedProductColor
            {
                Name = request.Name,
                HexCode = request.HexCode,
                MainImageUrl = imageUrl,
                DesignedProductId = request.ProductId
            };

            try
            {
                _context.DesignedProductColors.Add(newProductColor);
                await _context.SaveChangesAsync(cancellationToken);

                return Result.Success(new AddFactoryProductColorResponse(newProductColor.Id));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to save new product color to database. Rolling back uploaded image.");

                try
                {
                    await _imageService.DeleteAsync(imageUrl);
                }
                catch (Exception deleteEx)
                {
                    _logger.LogCritical(deleteEx, "Storage Leak: Failed to delete image {ImageUrl} after DB save failed.", imageUrl);
                }

                return Result.Failure<AddFactoryProductColorResponse>(new("FactoryProductColor.CreationError", "An error occurred while creating the product color.", 500));
            }
        }
    }
}
