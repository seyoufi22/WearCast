namespace WearCast.Api.Entities.Order;

public class CustomerDesignedOrderItem : BaseModel
{
    public int OrderId { get; set; }
    public int CustomerDesignId { get; set; }
    public int DesignedProductId { get; set; }

    // Snapshotted product data at purchase time
    public string ProductName { get; set; } = string.Empty;
    public string ColorName { get; set; } = string.Empty;
    public string SizeName { get; set; } = string.Empty;
    public int Quantity { get; set; }
    public decimal UnitPrice { get; set; }

    // Snapshotted design images
    public string? FrontImageUrl { get; set; }
    public string? BackImageUrl { get; set; }
    public string? RightImageUrl { get; set; }
    public string? LeftImageUrl { get; set; }
    public string ViewDesignsJson { get; set; } = string.Empty;

    public Order Order { get; set; } = null!;
}
