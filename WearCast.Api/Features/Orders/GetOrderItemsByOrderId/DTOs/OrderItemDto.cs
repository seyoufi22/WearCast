namespace WearCast.Api.Features.Orders.GetOrderItemsByOrderId.DTOs;

public class OrderItemDto
{
    public int Id { get; set; }
    public int FixedColorId { get; set; }
    public string ProductName { get; set; } = string.Empty;
    public string ColorName { get; set; } = string.Empty;
    public string SizeName { get; set; } = string.Empty;
    public int Quantity { get; set; }
    public decimal UnitPrice { get; set; }
    public decimal TotalPrice => UnitPrice * Quantity;
    public string ImageUrl { get; set; } = string.Empty;
}
