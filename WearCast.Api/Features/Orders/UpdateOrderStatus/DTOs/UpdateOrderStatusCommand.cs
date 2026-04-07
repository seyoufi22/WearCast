using WearCast.Api.Abstractions;
using WearCast.Api.Common.Enums;
using MediatR;

namespace WearCast.Api.Features.Orders.UpdateOrderStatus.DTOs;

public record UpdateOrderStatusCommand(
    int OrderId,
    OrderStatus NewStatus,
    int? SellerId = null,
    int? DriverId = null) : IRequest<Result<bool>>;
