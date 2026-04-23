namespace WearCast.Api.Features.FixedProduct.GetAllFixedProductsForAdmin.Query;

using WearCast.Api.Common.Helper;
using WearCast.Api.Common.Views;
using WearCast.Api.Features.FixedProduct.GetAllFixedProductsForAdmin.DTOs;
using MediatR;
using Microsoft.EntityFrameworkCore;

public class GetAllFixedProductsForAdminHandler : IRequestHandler<GetAllFixedProductsForAdminRequestDto, Result<PagingViewModel<GetAllFixedProductsForAdminResponseDto>>>
{
    private readonly IRepository<Entities.FixedProduct.FixedProduct> _productRepo;

    public GetAllFixedProductsForAdminHandler(IRepository<Entities.FixedProduct.FixedProduct> productRepo)
    {
        _productRepo = productRepo;
    }

    public async Task<Result<PagingViewModel<GetAllFixedProductsForAdminResponseDto>>> Handle(GetAllFixedProductsForAdminRequestDto request, CancellationToken cancellationToken)
    {
        var query = _productRepo.Get()
            .AsNoTracking()
            .Where(p => !p.IsDeleted);

        if (!string.IsNullOrWhiteSpace(request.SearchTerm))
        {
            var term = request.SearchTerm.Trim();
            query = query.Where(p =>
                p.Name.Contains(term) ||
                p.Seller.Name.Contains(term)||
                p.Description.Contains(term)); 
        }
        query = query.OrderByDescending(p => p.CreatedOn).ThenBy(p => p.Id);

        var projectedQuery = query.Select(p => new GetAllFixedProductsForAdminResponseDto
        {
            Id = p.Id,
            ProductName = p.Name,
            StoreName = p.Seller.Name, 
            Price = p.Price,
            IsRejected = !p.Colors.Any(c => !c.IsDeleted),

            MainImageUrl = p.Colors
                .Where(c => !c.IsDeleted && c.Sizes.Any(s => s.Quantity > 0))
                .OrderBy(c => c.Id)
                .Select(c => c.ImageUrl)
                .FirstOrDefault()
        });

        var pagedResult = await PagingHelper.CreateAsync(projectedQuery, request.PageIndex, request.PageSize);

        return Result.Success(pagedResult);
    }
}