using WearCast.Api.Common.Helper;
using WearCast.Api.Common.Tracking;
using WearCast.Api.Common.Tracking.Models;
using WearCast.Api.Common.Views;
using WearCast.Api.Features.FixedProduct.GetAllFixedProducts.DTOs;

namespace WearCast.Api.Features.FixedProduct.GetAllFixedProducts.Query;

public class GetAllFixedProductsHandler(ApplicationDbContext context,
        ITrackingService trackingService,
        IHttpContextAccessor httpContextAccessor,
        IRepository<Entities.FixedProduct.FixedProduct> productRepo) : IRequestHandler<GetAllFixedProductsQuery, Result<PagingViewModel<GetAllFixedProductsResponseDto>>>
{
    private readonly IRepository<Entities.FixedProduct.FixedProduct> _productRepo=productRepo;
    private readonly ApplicationDbContext _context = context;
    private readonly ITrackingService _trackingService = trackingService;
    private readonly IHttpContextAccessor _httpContextAccessor = httpContextAccessor;

    public async Task<Result<PagingViewModel<GetAllFixedProductsResponseDto>>> Handle(GetAllFixedProductsQuery request, CancellationToken cancellationToken)
    {
        var query = _productRepo.Get()
        .AsNoTracking()
        .Where(p => p.Colors.Any(c => !c.IsDeleted));
        if (!string.IsNullOrEmpty(request.SearchTerm))
        {
            var term = request.SearchTerm.Trim();
            query = query.Where(p => p.Name.Contains(term) ||
            p.Seller.Name.Contains(term) ||
            p.Description.Contains(term));
        }

        string? categoryNameToTrack = null;
        if (request.CategoryId.HasValue)
        {
            query = query.Where(p => p.Category.Id == request.CategoryId.Value);
            categoryNameToTrack = await _context.Categories
                    .Where(c => c.Id == request.CategoryId.Value)
                    .Select(c => c.Name)
                    .FirstOrDefaultAsync(cancellationToken);
        }

        if(request.DressStyle.HasValue)
            query = query.Where(p => p.DressStyle == request.DressStyle.Value);

        if (request.TargetAudience.HasValue)
            query = query.Where(p => (p.TargetAudience&request.TargetAudience.Value) == request.TargetAudience.Value);

        if (request.MinPrice.HasValue)
            query = query.Where(p => p.Price >= request.MinPrice.Value);

        if (request.MaxPrice.HasValue)
            query = query.Where(p => p.Price <= request.MaxPrice.Value);

        if (request.Sizes != null && request.Sizes.Any()) 
        {
            query = query.Where(p => p.Colors.Any(c =>
                !c.IsDeleted &&
                c.Sizes.Any(s => request.Sizes.Contains(s.Size) && s.Quantity > 0)
            ));
        }
        else
        {
            query = query.Where(p => p.Colors.Any(c =>
                !c.IsDeleted &&
                c.Sizes.Any(s => s.Quantity > 0)
            ));
        }

        query = request.SortBy switch
        {
            SortBy.PriceAsc => query.OrderBy(p => p.Price),
            SortBy.PriceDesc => query.OrderByDescending(p => p.Price),
            //SortBy.MostPopular => query.OrderByDescending(p => p.SalesCount),
            SortBy.Newest => query.OrderByDescending(p => p.CreatedOn),
            _ => query
        };

        var projectedQuery = query.Select(product => new
        {
            Product = product,
            FirstColor = product.Colors
            .Where(c => !c.IsDeleted && c.Sizes.Any(s => s.Quantity > 0))
            .OrderBy(c => c.Id)
            .Select(c => new { c.Id, c.ImageUrl })
            .FirstOrDefault()
        })
        .Select(x => new GetAllFixedProductsResponseDto
        {
            Id = x.Product.Id,
            CategoryId = x.Product.CategoryId,
            Name = x.Product.Name,
            Price = x.Product.Price,
            TargetAudience = x.Product.TargetAudience,
            colorId = x.FirstColor != null ? x.FirstColor.Id : 0,
            MainImageUrl = x.FirstColor != null ? x.FirstColor.ImageUrl : null
        });
        var user = _httpContextAccessor.HttpContext?.User;
        if (user != null && user.IsCustomer())
        {
            var userId = user.GetUserId();

            if (!string.IsNullOrEmpty(userId))
            {
                var filterEvent = new FilterEvent
                {
                    UserId = userId,
                    Filters = new FilterDetails
                    {
                        SearchKey = request.SearchTerm,
                        MinPrice = request.MinPrice,
                        MaxPrice = request.MaxPrice,

                        TargetAudience = request.TargetAudience?.ToString().Split(", ").ToList(),
                        
                        DressStyle = request.DressStyle?.ToString(),
                        CategoryName = categoryNameToTrack,
                        SellerId = null
                    }
                };

                _trackingService.TrackFilter(filterEvent);
            }
        }

        var pagedResult = await PagingHelper.CreateAsync(projectedQuery, request.PageIndex, request.PageSize);

        return Result.Success(pagedResult);
    }
}     