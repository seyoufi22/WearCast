using WearCast.Api.Abstractions;
using WearCast.Api.Common.Helper;
using WearCast.Api.Persistence;
using WearCast.Api.Common.Views;
using WearCast.Api.Entities.FixedProduct;
using WearCast.Api.Features.Favourites.GetAllFavouritesByCustomerId.DTOs;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace WearCast.Api.Features.Favourites.GetAllFavouritesByCustomerId.Query;

public class GetAllFavouritesByCustomerIdHandler : IRequestHandler<GetAllFavouritesByCustomerIdQuery, Result<PagingViewModel<FavouriteItemDto>>>
{
    private readonly ApplicationDbContext _context;

    public GetAllFavouritesByCustomerIdHandler(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<Result<PagingViewModel<FavouriteItemDto>>> Handle(GetAllFavouritesByCustomerIdQuery request, CancellationToken cancellationToken)
    {
        var query = _context.Favourites
            .Where(f => f.CustomerId == request.CustomerId)
            .Include(f => f.FixedProductColor)
                .ThenInclude(fc => fc.Product)
            .AsNoTracking();

        var projectedQuery = query.Select(f => new FavouriteItemDto
        {
            CustomerId = f.CustomerId,
            FixedProductColorId = f.FixedProductColorId,
            ProductId = f.FixedProductColor.ProductId,
            ProductName = f.FixedProductColor.Product.Name,
            ColorName = f.FixedProductColor.ColorName,
            ImageUrl = f.FixedProductColor.ImageUrl,
            Price = f.FixedProductColor.Product.Price
        });

        var pagedResult = await PagingHelper.CreateAsync(projectedQuery, request.PageIndex, request.PageSize);

        return Result.Success(pagedResult);
    }
}
