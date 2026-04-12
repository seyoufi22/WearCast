using WearCast.Api.Common.Tracking;
using WearCast.Api.Common.Tracking.Models;

namespace WearCast.Api.Features.DesignedProductManagement.CustomerCatalogAndWorkspace.GetProductDetails
{
    public class GetProductDetailsHandler(
        ApplicationDbContext context,
        ITrackingService trackingService,
        IHttpContextAccessor httpContextAccessor
        ) : IRequestHandler<GetProductDetailsRequest, Result<GetProductDetailsResponse>>
    {
        private readonly ApplicationDbContext _context = context;
        private readonly ITrackingService _trackingService = trackingService;
        private readonly IHttpContextAccessor _httpContextAccessor = httpContextAccessor;

        public async Task<Result<GetProductDetailsResponse>> Handle(GetProductDetailsRequest request, CancellationToken cancellationToken)
        {
            var user = _httpContextAccessor.HttpContext!.User;

            var product = await _context.DesignedProducts
                            .Include(p => p.Category)
                            .Include(p => p.Colors)
                                .ThenInclude(c => c.Images)
                            .Include(p => p.SizeDetails)
                            .AsNoTracking()
                            .FirstOrDefaultAsync(p => p.Id == request.ProductId && !p.IsDeleted, cancellationToken);

            if (product == null)
            {
                return Result.Failure<GetProductDetailsResponse>(
                   new Error("Catalog.ProductNotFound", "The requested product was not found or is no longer available.", StatusCodes.Status404NotFound));
            }


            int targetColorId = request.DefaultColorId
                ?? product.DefaultColorId
                ?? product.Colors.FirstOrDefault()?.Id
                ?? 0;

            var orderedColors = product.Colors
                .OrderByDescending(c => c.Id == targetColorId)
                .ThenBy(c => c.Id)
                .Select(c => new ColorVariantResponse(
                    c.Id,
                    c.Name,
                    c.HexCode,
                    c.MainImageUrl,
                    c.Images.Select(img => new ImageResponse(img.ImageUrl, img.ViewSide.ToString())).ToList()
                )).ToList();

            var response = new GetProductDetailsResponse(
                product.Id,
                product.Name,
                product.Description,
                product.TargetAudience.ToString().Split(", ").ToList(),
                product.Price,
                product.SalesCount,
                product.CanvasWidth,
                product.CanvasHeight,

                new CategoryResponse(
                    product.Category.Name,
                    product.Category.ImageUrl
                ),

                product.SizeDetails
                    .OrderBy(s => s.Size)
                    .Select(s => new SizeDetailsResponse(s.Size.ToString(), s.A, s.B, s.C))
                    .ToList(),

                orderedColors
            );


            if (user.IsCustomer())
            {
                var userId = user.GetUserId();

                if (!string.IsNullOrEmpty(userId))
                {
                    var clickEvent = new ClickEvent
                    {
                        UserId = userId,
                        ProductDetails = new ProductDetails
                        {
                            Price = product.Price,
                            TargetAudience = product.TargetAudience.ToString().Split(", ").ToList(),
                            DressStyle = product.DressStyle.ToString(),
                            CategoryName = product.Category?.Name,
                            SellerId = null
                        }
                    };

                    _trackingService.TrackClick(clickEvent);
                }
            }

            return Result.Success(response);
        }
    }
}
