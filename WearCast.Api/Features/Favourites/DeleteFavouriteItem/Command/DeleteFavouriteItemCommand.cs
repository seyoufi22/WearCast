using WearCast.Api.Abstractions;
using MediatR;

namespace WearCast.Api.Features.Favourites.DeleteFavouriteItem.Command;

public record DeleteFavouriteItemCommand(int CustomerId, int FixedProductColorId) : IRequest<Result<Unit>>;
