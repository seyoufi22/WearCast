namespace WearCast.Api.Features.FixedProduct.GetAllFixedProductsStatusForSeller.Query;
using WearCast.Api.Features.FixedProduct.GetAllFixedProductsStatusForSeller.DTOs;
public class GetAllFixedProductsStatusForSellerHandler : IRequestHandler<GetAllFixedProductsStatusForSellerRequestDto, Result<GetAllFixedProductsStatusForSellerResponseDto>>
{
    private readonly IRepository<Entities.FixedProduct.FixedProduct> _productRepo;
    public GetAllFixedProductsStatusForSellerHandler(IRepository<Entities.FixedProduct.FixedProduct> productRepo)
    {
        _productRepo = productRepo;
    }
    public async Task<Result<GetAllFixedProductsStatusForSellerResponseDto>> Handle(GetAllFixedProductsStatusForSellerRequestDto request, CancellationToken cancellationToken)
    {
        var baseQuery = _productRepo.Get()
            .AsNoTracking()
            .Where(p => !p.IsDeleted && p.SellerId == request.SellerId);

        var totalProducts = await baseQuery.CountAsync(cancellationToken);

        if (totalProducts == 0)
        {
            return Result.Success(new GetAllFixedProductsStatusForSellerResponseDto());
        }

        var approved = await baseQuery.CountAsync(p => p.Colors.Any(c => !c.IsDeleted), cancellationToken);

        var rejected =  totalProducts - approved;

        var lowStock = await baseQuery.CountAsync(p =>
            p.Colors.Where(c => !c.IsDeleted).SelectMany(c => c.Sizes).Sum(s => s.Quantity) > 0 &&
            p.Colors.Any(c => !c.IsDeleted && c.Sizes.Sum(s => s.Quantity) <= 10),
            cancellationToken);

        var response = new GetAllFixedProductsStatusForSellerResponseDto
        {
            TotalProducts = totalProducts,
            Approved = approved,
            Rejected = rejected,
            LowStock = lowStock
        };

        return Result.Success(response);
    }
}
