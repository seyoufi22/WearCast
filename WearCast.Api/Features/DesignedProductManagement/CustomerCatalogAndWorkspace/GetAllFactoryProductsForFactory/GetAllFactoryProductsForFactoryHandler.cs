using WearCast.Api.Common.Helper;
using WearCast.Api.Common.Views;

namespace WearCast.Api.Features.DesignedProductManagement.CustomerCatalogAndWorkspace.GetAllFactoryProductsForFactory
{
    public class GetAllFactoryProductsForFactoryHandler(
        ApplicationDbContext context
        ) : IRequestHandler<GetAllFactoryProductsForFactoryRequest, Result<PagingViewModel<GetAllFactoryProductsForFactoryResponse>>>
    {
        private readonly ApplicationDbContext _context = context;
        public async Task<Result<PagingViewModel<GetAllFactoryProductsForFactoryResponse>>> Handle(GetAllFactoryProductsForFactoryRequest request, CancellationToken cancellationToken)
        {
            var query = _context.DesignedProducts
                            .AsNoTracking();

            if (!string.IsNullOrWhiteSpace(request.SearchTerm))
            {
                var search = request.SearchTerm.Trim().ToLower();
                query = query.Where(p => p.Name.ToLower().Contains(search) ||
                                         p.Description.ToLower().Contains(search));
            }

            if (request.CategoryId.HasValue)
                query = query.Where(p => p.CategoryId == request.CategoryId.Value);

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
                SortBy.Newest => query.OrderByDescending(p => p.CreatedOn),
                _ => query.OrderByDescending(p => p.CreatedOn)
            };

            var projectedQuery = query.Select(p => new GetAllFactoryProductsForFactoryResponse
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
                    : p.Colors.Select(c => c.MainImageUrl).FirstOrDefault()
            });

            var pagedResult = await PagingHelper.CreateAsync(projectedQuery, request.PageIndex, request.PageSize);

            return Result.Success(pagedResult);
        }
    }
}
