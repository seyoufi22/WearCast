using WearCast.Api.Abstractions;
using WearCast.Api.Common.Enums;
using WearCast.Api.Common.Views;
using WearCast.Api.Features.Orders.GetAllOrders.DTOs;
using MediatR;

namespace WearCast.Api.Features.Orders.GetAllOrders.Query;

public record GetAllOrdersQuery(
    int? SellerId,
    int? FactoryId,
    int PageNumber = 1,
    int PageSize = 10,
    OrderStatus? StatusFilter = null,
    string? SortBy = null,
    bool SortDescending = true) : IRequest<Result<PagingViewModel<GetAllOrdersResponseDto>>>;
