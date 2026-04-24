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

        if(!string.IsNullOrEmpty(request.SearchTerm))
            query = query.Where(p => p.Name.Contains(request.SearchTerm.Trim()));

        if (request.CategoryId.HasValue)
            query = query.Where(p => p.Category.Id == request.CategoryId.Value);

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

        var pagedResult = await PagingHelper.CreateAsync(projectedQuery, request.PageIndex, request.PageSize);

        return Result.Success(pagedResult);
    }
}   