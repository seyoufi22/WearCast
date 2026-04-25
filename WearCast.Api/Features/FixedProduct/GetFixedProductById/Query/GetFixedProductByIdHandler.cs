using WearCast.Api.Common.Tracking;
using WearCast.Api.Common.Tracking.Models;
using WearCast.Api.Features.FixedProduct.Errors;
using WearCast.Api.Features.FixedProduct.GetFixedProductById.DTOs;

namespace WearCast.Api.Features.FixedProduct.GetFixedProductById.Query;

public class GetFixedProductByIdHandler(ApplicationDbContext context,
        ITrackingService trackingService,
        IHttpContextAccessor httpContextAccessor,
        IRepository<Entities.FixedProduct.FixedProduct> productRepo) : IRequestHandler<GetFixedProductByIdQuery, Result<GetFixedProductByIdResponseDto>>
{
    private readonly IRepository<Entities.FixedProduct.FixedProduct> _productRepo = productRepo;
    private readonly ApplicationDbContext _context = context;
    private readonly ITrackingService _trackingService = trackingService;
    private readonly IHttpContextAccessor _httpContextAccessor = httpContextAccessor;

    public async Task<Result<GetFixedProductByIdResponseDto>> Handle(GetFixedProductByIdQuery request, CancellationToken cancellationToken)
    {
        var product = await _productRepo.GetAsync(p => p.Id == request.Id, useNoTracking: true);

        if (product == null)
        {
            return Result.Failure<GetFixedProductByIdResponseDto>(FixedProductErrors.ProductNotFound);
        }

        var response = new GetFixedProductByIdResponseDto
        {
            Id = product.Id,
            CategoryId = product.CategoryId,
            Name = product.Name,
            Price = product.Price,
            Description = product.Description,
            TargetAudience = product.TargetAudience.ToString(),
            DressStyle = product.DressStyle.ToString(),
            SellerId = product.SellerId,
            SizeDetails = product.SizeDetails.Select(sd => new ProductSizeDetailResponseDto
            {
                Size = sd.Size.ToString(),
                A = sd.A,
                B = sd.B,
                C = sd.C
            }).ToList()
        };

        var user = _httpContextAccessor.HttpContext!.User;
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
                        SellerId = product.Seller.Id
                    }
                };

                _trackingService.TrackClick(clickEvent);
            }
        }

        return Result.Success(response);
    }
}
