namespace WearCast.Api.Features.Orders.GetOrderItemsByShipmentId.DTOs;

public class FixedProductOrderItemDto
{
    public int FixedColorId { get; set; }
    public string ProductName { get; set; } = string.Empty;
    public string ColorName { get; set; } = string.Empty;
    public decimal UnitPrice { get; set; }
    public string ImageUrl { get; set; } = string.Empty;
    public int TotalQuantity { get; set; }
    public decimal TotalPrice { get; set; }
    public List<OrderItemSizeDto> Sizes { get; set; } = new();
}

public class OrderItemSizeDto
{
    public int Id { get; set; }
    public int OrderId { get; set; }
    public string SizeName { get; set; } = string.Empty;
    public int Quantity { get; set; }
}
