using WearCast.Api.Abstractions;
using WearCast.Api.Common.Enums;
using WearCast.Api.Common.Views;
using WearCast.Api.Features.Orders.GetOrdersByCustomerId.DTOs;
using MediatR;

namespace WearCast.Api.Features.Orders.GetOrdersByCustomerId.Query;

public record GetOrdersByCustomerIdQuery(
    int CustomerId,
    int PageNumber = 1,
    int PageSize = 10,
    OrderStatus? StatusFilter = null,
    string? SortBy = null,
    bool SortDescending = true) : IRequest<Result<PagingViewModel<GetOrdersByCustomerIdResponseDto>>>;
