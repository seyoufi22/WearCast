using WearCast.Api.Features.FixedProduct.GetAllFixedProducts.DTOs;
using WearCast.Api.Common.Helper;
using WearCast.Api.Common.Views;

namespace WearCast.Api.Features.FixedProduct.GetAllFixedProducts.Query;

public class GetAllFixedProductsHandler : IRequestHandler<GetAllFixedProductsQuery, Result<PagingViewModel<GetAllFixedProductsResponseDto>>>
{
    private readonly IRepository<Entities.FixedProduct.FixedProduct> _productRepo;

    public GetAllFixedProductsHandler(IRepository<Entities.FixedProduct.FixedProduct> productRepo)
    {
        _productRepo = productRepo;
    }

    public async Task<Result<PagingViewModel<GetAllFixedProductsResponseDto>>> Handle(GetAllFixedProductsQuery request, CancellationToken cancellationToken)
    {
        var query = _productRepo.Get()
        .AsNoTracking()
        .Where(p => p.Colors.Any(c => !c.IsDeleted));
        //var query = _productRepo.Get().Include(p => p.SizeDetails).AsNoTracking();

        if (!string.IsNullOrEmpty(request.Category))
            query = query.Where(p => p.Category.Name == request.Category);

        if (request.TargetAudience.HasValue)
            query = query.Where(p => p.TargetAudience == request.TargetAudience.Value);

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
        var projectedQuery = query.Select(product => new GetAllFixedProductsResponseDto
        {
            Id = product.Id,
            CategoryId = product.CategoryId,
            Name = product.Name,
            Price = product.Price,
            Description = product.Description,
            TargetAudience = product.TargetAudience,
            MainImageUrl = product.Colors
            .Where(c => !c.IsDeleted && c.Sizes.Any(s => s.Quantity > 0))
            .OrderBy(c => c.Id)
            .Select(c => c.ImageUrl)
            .FirstOrDefault(),
        });

        var pagedResult = await PagingHelper.CreateAsync(projectedQuery, request.PageIndex, request.PageSize);

        return Result.Success(pagedResult);
    }
}
