using WearCast.Api.Common.Enums;

namespace WearCast.Api.Features.Orders.UpdateOrderStatus.DTOs;

public class UpdateOrderStatusRequestDto
{
    public OrderStatus NewStatus { get; set; }
}
