using WearCast.Api.Entities.FixedProduct;

namespace WearCast.Api.Entities.Order;

public class FixedProductOrderItem : BaseModel
{
    public int OrderId { get; set; }
    public int FixedColorId { get; set; }

    // Snapshotted product data at purchase time
    public string ProductName { get; set; } = string.Empty;
    public string ColorName { get; set; } = string.Empty;
    public string ImageUrl { get; set; } = string.Empty;
    public string SizeName { get; set; } = string.Empty;
    public int Quantity { get; set; }
    public decimal UnitPrice { get; set; }

    public Order Order { get; set; } = null!;
    public FixedProductColor FixedColor { get; set; } = null!;
}
