using WearCast.Api.Abstractions;
using MediatR;

namespace WearCast.Api.Features.Favourites.AddNewFavouriteItem.Command;

public record AddNewFavouriteItemCommand(int CustomerId, int FixedProductColorId) : IRequest<Result<Unit>>;
