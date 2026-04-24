namespace WearCast.Api.Features.Orders.GetOrderItemsByOrderId.DTOs;

public class DesignedOrderItemDto
{
    public int Id { get; set; }
    public int CustomerDesignId { get; set; }
    public string ProductName { get; set; } = string.Empty;
    public string ColorName { get; set; } = string.Empty;
    public string SizeName { get; set; } = string.Empty;
    public int Quantity { get; set; }
    public decimal UnitPrice { get; set; }
    public decimal TotalPrice => UnitPrice * Quantity;

    // Design images fetched via CustomerDesign FK
    public string? FrontImageUrl { get; set; }
    public string? BackImageUrl { get; set; }
    public string? RightImageUrl { get; set; }
    public string? LeftImageUrl { get; set; }
    public string ViewDesignsJson { get; set; } = string.Empty;
}
