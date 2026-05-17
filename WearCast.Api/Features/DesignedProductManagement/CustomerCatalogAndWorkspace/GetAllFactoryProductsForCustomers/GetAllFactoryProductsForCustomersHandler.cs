using WearCast.Api.Common.Helper;
using WearCast.Api.Common.Tracking;
using WearCast.Api.Common.Tracking.Models;
using WearCast.Api.Common.Views;

namespace WearCast.Api.Features.DesignedProductManagement.CustomerCatalogAndWorkspace.GetAllFactoryProductsForCustomers
{
    public class GetAllFactoryProductsForCustomersHandler(
        ApplicationDbContext context,
        ITrackingService trackingService,
        IHttpContextAccessor httpContextAccessor
        ) : IRequestHandler<GetAllFactoryProductsForCustomersRequest, Result<PagingViewModel<GetAllFactoryProductsForCustomersResponse>>>
    {
        private readonly ApplicationDbContext _context = context;
        private readonly ITrackingService _trackingService = trackingService;
        private readonly IHttpContextAccessor _httpContextAccessor = httpContextAccessor;

        public async Task<Result<PagingViewModel<GetAllFactoryProductsForCustomersResponse>>> Handle(GetAllFactoryProductsForCustomersRequest request, CancellationToken cancellationToken)
        {
            var user = _httpContextAccessor.HttpContext?.User;

            var query = _context.DesignedProducts
                            .AsNoTracking()
                            .Where(p => p.Colors.Any());

            if (!string.IsNullOrWhiteSpace(request.SearchTerm))
            {
                var search = request.SearchTerm.Trim().ToLower();
                query = query.Where(p => p.Name.ToLower().Contains(search) ||
                                         p.Description.ToLower().Contains(search));
            }

            string? categoryNameToTrack = null;

            if (request.CategoryId.HasValue)
            {
                query = query.Where(p => p.CategoryId == request.CategoryId.Value);

                categoryNameToTrack = await _context.Categories
                    .Where(c => c.Id == request.CategoryId.Value)
                    .Select(c => c.Name)
                    .FirstOrDefaultAsync(cancellationToken);
            }

            if (request.DressStyle.HasValue)
                query = query.Where(p => p.DressStyle == request.DressStyle.Value);

            if (request.TargetAudiences.HasValue)
            {
                int mask = (int)request.TargetAudiences.Value;
                query = query.Where(p => ((int)p.TargetAudience & mask) != 0);
            }

            if (request.MinPrice.HasValue)
                query = query.Where(p => p.Price >= request.MinPrice.Value);

            if (request.MaxPrice.HasValue)
                query = query.Where(p => p.Price <= request.MaxPrice.Value);

            query = request.SortBy switch
            {
                SortBy.PriceAsc => query.OrderBy(p => p.Price),
                SortBy.PriceDesc => query.OrderByDescending(p => p.Price),
                SortBy.BestSeller => query.OrderByDescending(p => p.SalesCount),
                SortBy.MostPopular => query.OrderByDescending(p => p.AverageRating)
                                           .ThenByDescending(p => p.ReviewCount),
                SortBy.Newest => query.OrderByDescending(p => p.CreatedOn),
                _ => query.OrderByDescending(p => p.CreatedOn)
            };

            var projectedQuery = query.Select(p => new GetAllFactoryProductsForCustomersResponse
            {
                Id = p.Id,
                CategoryId = p.CategoryId,
                CategoryName = p.Category.Name,
                Name = p.Name,
                Price = p.Price,
                TargetAudienceEnum = p.TargetAudience,
                DefaultColorId = p.DefaultColorId,
                MainImageUrl = p.DefaultColor != null
                    ? p.DefaultColor.MainImageUrl
                    : p.Colors.Select(c => c.MainImageUrl).FirstOrDefault(),

                AverageRating = p.AverageRating,
                ReviewCount = p.ReviewCount
            });

            var pagedResult = await PagingHelper.CreateAsync(projectedQuery, request.PageIndex, request.PageSize);

            if (user?.Identity?.IsAuthenticated == true && user.IsCustomer())
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

                            TargetAudience = request.TargetAudiences?.ToString().Split(", ").ToList(),

                            DressStyle = request.DressStyle?.ToString(),
                            CategoryName = categoryNameToTrack,
                            SellerId = null
                        }
                    };

                    _trackingService.TrackFilter(filterEvent);
                }
            }

            return Result.Success(pagedResult);
        }
    }
}