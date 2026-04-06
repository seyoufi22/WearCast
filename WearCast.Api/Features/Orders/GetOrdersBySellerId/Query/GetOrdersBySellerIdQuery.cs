using WearCast.Api.Abstractions;
using WearCast.Api.Common.Views;
using WearCast.Api.Features.Orders.GetOrdersBySellerId.DTOs;
using MediatR;

namespace WearCast.Api.Features.Orders.GetOrdersBySellerId.Query;

public record GetOrdersBySellerIdQuery(int SellerId, int PageNumber = 1, int PageSize = 10) : IRequest<Result<PagingViewModel<GetOrdersBySellerIdResponseDto>>>;
