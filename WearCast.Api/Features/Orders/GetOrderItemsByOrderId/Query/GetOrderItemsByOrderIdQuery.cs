using WearCast.Api.Abstractions;
using WearCast.Api.Features.Orders.GetOrderItemsByOrderId.DTOs;
using MediatR;

namespace WearCast.Api.Features.Orders.GetOrderItemsByOrderId.Query;

public record GetOrderItemsByOrderIdQuery(int OrderId, int? CustomerId, int? SellerId, int? FactoryId) : IRequest<Result<GetOrderItemsByOrderIdResponseDto>>;
