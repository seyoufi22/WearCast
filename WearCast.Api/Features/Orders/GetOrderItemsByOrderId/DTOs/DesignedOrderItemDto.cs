namespace WearCast.Api.Features.Orders.GetOrderItemsByOrderId.DTOs;

public class DesignedOrderItemDto
{
    public int CustomerDesignId { get; set; }
    public string ProductName { get; set; } = string.Empty;
    public string ColorName { get; set; } = string.Empty;
    public decimal UnitPrice { get; set; }
    public int TotalQuantity { get; set; }
    public decimal TotalPrice { get; set; }

    public string? FrontImageUrl { get; set; }
    public string? BackImageUrl { get; set; }
    public string? RightImageUrl { get; set; }
    public string? LeftImageUrl { get; set; }
    public string ViewDesignsJson { get; set; } = string.Empty;
    public List<OrderItemSizeDto> Sizes { get; set; } = new();
}
