using MediatR;
using WearCast.Api.Common.Views;

namespace WearCast.Api.Features.Sellers.GetAllSellers;

public record GetAllSellersRequest(
    int PageIndex = 1,
    int PageSize = 10,
    string? SearchTerm = null
) : IRequest<Result<PagingViewModel<GetAllSellersResponse>>>;