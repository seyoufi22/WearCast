using WearCast.Api.Abstractions;
using WearCast.Api.Features.Orders.GetOrdersByShipmentId.DTOs;
using MediatR;

namespace WearCast.Api.Features.Orders.GetOrdersByShipmentId.Query;

public record GetOrdersByShipmentIdQuery(int ShipmentId, int? CustomerId, bool IsAdmin) : IRequest<Result<GetOrdersByShipmentIdResponseDto>>;
