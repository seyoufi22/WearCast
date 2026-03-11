using WearCast.Api.Abstractions;
using WearCast.Api.Persistence;
using WearCast.Api.Entities.FixedProduct;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace WearCast.Api.Features.Favourites.AddNewFavouriteItem.Command;

public class AddNewFavouriteItemHandler : IRequestHandler<AddNewFavouriteItemCommand, Result<Unit>>
{
    private readonly ApplicationDbContext _context;

    public AddNewFavouriteItemHandler(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<Result<Unit>> Handle(AddNewFavouriteItemCommand request, CancellationToken cancellationToken)
    {
        var existingFavourite = await _context.Favourites.AsNoTracking().FirstOrDefaultAsync(
            f => f.CustomerId == request.CustomerId && f.FixedProductColorId == request.FixedProductColorId, cancellationToken);

        if (existingFavourite != null)
        {
            return Result.Failure<Unit>(FavouriteErrors.FavouriteAlreadyExists);
        }

        var newFavourite = new Favourite
        {
            CustomerId = request.CustomerId,
            FixedProductColorId = request.FixedProductColorId
        };

        _context.Favourites.Add(newFavourite);
        await _context.SaveChangesAsync(cancellationToken);

        return Result.Success(Unit.Value);
    }
}
