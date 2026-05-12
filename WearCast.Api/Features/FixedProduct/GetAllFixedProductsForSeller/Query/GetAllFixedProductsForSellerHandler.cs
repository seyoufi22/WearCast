namespace WearCast.Api.Features.FixedProduct.GetAllFixedProductsForSeller.Query;

using WearCast.Api.Common.Helper;
using WearCast.Api.Common.Views;
using WearCast.Api.Features.FixedProduct.GetAllFixedProductsForSeller.DTOs;
public class GetAllFixedProductsForSellerHandler : IRequestHandler<GetAllFixedProductsForSellerRequestDto, Result<PagingViewModel<GetAllFixedProductsForSellerResponseDto>>>
{
    private readonly IRepository<Entities.FixedProduct.FixedProduct> _productRepo;
    public GetAllFixedProductsForSellerHandler(IRepository<Entities.FixedProduct.FixedProduct> productRepo)
    {
        _productRepo = productRepo;
    }
    public async Task<Result<PagingViewModel<GetAllFixedProductsForSellerResponseDto>>> Handle(GetAllFixedProductsForSellerRequestDto request, CancellationToken cancellationToken)
    {
        var query = _productRepo.Get()
        .AsNoTracking()
        .Where(p => !p.IsDeleted && p.SellerId == request.SellerId);

        if (request.CategoryId.HasValue)
            query = query.Where(p => p.CategoryId == request.CategoryId.Value);

        if(request.IsRejected.HasValue)
        {
            if (request.IsRejected.Value)
            {
                query = query.Where(p => !p.Colors.Any(c => !c.IsDeleted));
            }
            else
            {
                query = query.Where(p => p.Colors.Any(c => !c.IsDeleted));
            }
        }

        if (request.StockStatus.HasValue)
        {
            if (request.StockStatus.Value == StockStatus.OutOfStock)
            {
                query = query.Where(p =>
                    p.Colors.Where(c => !c.IsDeleted)
                            .SelectMany(c => c.Sizes)
                            .Sum(s => s.Quantity) == 0);
            }
            else if (request.StockStatus.Value == StockStatus.InStock)
            {
                query = query.Where(p =>
                    p.Colors.Any(c => !c.IsDeleted) &&
                    !p.Colors.Any(c => !c.IsDeleted && c.Sizes.Sum(s => s.Quantity) <= 10));
            }
            else 
            {
                query = query.Where(p =>
                    p.Colors.Where(c => !c.IsDeleted).SelectMany(c => c.Sizes).Sum(s => s.Quantity) > 0 &&
                    p.Colors.Any(c => !c.IsDeleted && c.Sizes.Sum(s => s.Quantity) <= 10));
            }
        }

        if (!string.IsNullOrWhiteSpace(request.SearchTerm))
        {
            var term = request.SearchTerm.Trim();
            query = query.Where(p => p.Name.Contains(term));
        }

        query = query.OrderByDescending(p => p.CreatedOn).ThenBy(p => p.Id);

        var projectedQuery = query.Select(product => new
        {
            Product = product,

            TotalStock = product.Colors
                .Where(c => !c.IsDeleted)
                .SelectMany(c => c.Sizes)
                .Sum(s => s.Quantity),

            HasActiveColors = product.Colors.Any(c => !c.IsDeleted),

            MainImageUrl = product.Colors
                .Where(c => !c.IsDeleted && c.Sizes.Any(s => s.Quantity > 0))
                .OrderBy(c => c.Id)
                .Select(c => c.ImageUrl)
                .FirstOrDefault()
        })
        .Select(x => new GetAllFixedProductsForSellerResponseDto
        {
            Id = x.Product.Id,
            Name = x.Product.Name,
            TargetAudience = x.Product.TargetAudience,
            Price = x.Product.Price,

            Stock = x.TotalStock,
            IsRejected = !x.HasActiveColors,
            MainImageUrl = x.MainImageUrl
        });
        var pagedResult = await PagingHelper.CreateAsync(projectedQuery, request.PageIndex, request.PageSize);

        return Result.Success(pagedResult);
    }
}
