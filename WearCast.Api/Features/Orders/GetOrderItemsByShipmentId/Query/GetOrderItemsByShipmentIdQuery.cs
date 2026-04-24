using WearCast.Api.Abstractions;
using WearCast.Api.Features.Orders.GetOrderItemsByShipmentId.DTOs;
using MediatR;

namespace WearCast.Api.Features.Orders.GetOrderItemsByShipmentId.Query;

public record GetOrderItemsByShipmentIdQuery(
    int ShipmentId,
    int? CustomerId,
    int? SellerId,
    int? FactoryId,
    int PageNumber = 1,
    int PageSize = 10,
    string? SearchTerm = null,
    string? SortBy = null,
    bool SortDescending = false
) : IRequest<Result<GetOrderItemsByShipmentIdResponseDto>>;
