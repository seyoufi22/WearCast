using WearCast.Api.Common.Enums;
using WearCast.Api.Common.ValueObjects;

namespace WearCast.Api.Features.Orders.GetOrdersByCustomerId.DTOs;

public class GetOrdersByCustomerIdResponseDto
{
    public int Id { get; set; }
    public decimal TotalAmount { get; set; }
    public OrderStatus Status { get; set; }
    public DateTime CreatedOn { get; set; }
    
    public string RecipientName { get; set; } = string.Empty;
    public string RecipientPhoneNumber { get; set; } = string.Empty;
    public Address ShippingAddress { get; set; } = new();
}
