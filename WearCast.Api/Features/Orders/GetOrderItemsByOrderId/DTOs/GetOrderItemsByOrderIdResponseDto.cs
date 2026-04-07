using WearCast.Api.Common.Enums;
using WearCast.Api.Common.ValueObjects;

namespace WearCast.Api.Features.Orders.GetOrderItemsByOrderId.DTOs;

public class GetOrderItemsByOrderIdResponseDto
{
    public int Id { get; set; }
    public decimal TotalAmount { get; set; }
    public OrderStatus Status { get; set; }
    public DateTime CreatedOn { get; set; }
    
    public string RecipientName { get; set; } = string.Empty;
    public string RecipientPhoneNumber { get; set; } = string.Empty;
    public string? RecipientAdditionalPhoneNumber { get; set; }
    public Address ShippingAddress { get; set; } = new();

    public List<OrderItemDto> Items { get; set; } = new();
}
