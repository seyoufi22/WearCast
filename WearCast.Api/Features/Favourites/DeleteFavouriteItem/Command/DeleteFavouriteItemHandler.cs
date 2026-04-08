using WearCast.Api.Abstractions;
using WearCast.Api.Persistence;
using WearCast.Api.Entities.FixedProduct;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace WearCast.Api.Features.Favourites.DeleteFavouriteItem.Command;

public class DeleteFavouriteItemHandler : IRequestHandler<DeleteFavouriteItemCommand, Result<Unit>>
{
    private readonly ApplicationDbContext _context;

    public DeleteFavouriteItemHandler(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<Result<Unit>> Handle(DeleteFavouriteItemCommand request, CancellationToken cancellationToken)
    {
        var existingFavourite = await _context.Favourites.FirstOrDefaultAsync(
            f => f.CustomerId == request.CustomerId && f.FixedProductColorId == request.FixedProductColorId, cancellationToken);

        if (existingFavourite == null)
        {
            return Result.Failure<Unit>(FavouriteErrors.FavouriteNotFound);
        }

        _context.Favourites.Remove(existingFavourite);
        await _context.SaveChangesAsync(cancellationToken);

        return Result.Success(Unit.Value);
    }
}
