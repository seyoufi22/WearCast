using WearCast.Api.Abstractions;
using WearCast.Api.Common.Views;
using WearCast.Api.Features.Favourites.GetAllFavouritesByCustomerId.DTOs;
using MediatR;

namespace WearCast.Api.Features.Favourites.GetAllFavouritesByCustomerId.Query;

public record GetAllFavouritesByCustomerIdQuery(int CustomerId, int PageIndex, int PageSize) : IRequest<Result<PagingViewModel<FavouriteItemDto>>>;
