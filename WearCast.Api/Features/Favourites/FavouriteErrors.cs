using WearCast.Api.Abstractions;

namespace WearCast.Api.Features.Favourites;

public static class FavouriteErrors
{
    public static readonly Error FavouriteAlreadyExists =
        new("Favourite.AlreadyExists", "This item is already in the user's favourites.", StatusCodes.Status409Conflict);

    public static readonly Error FavouriteNotFound =
        new("Favourite.NotFound", "This favourite item was not found.", StatusCodes.Status404NotFound);
}
