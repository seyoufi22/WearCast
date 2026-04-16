using WearCast.Api.Entities.DesignedProducts;

namespace WearCast.Api.Entities.Order;

public class CustomerDesignedOrderItem : BaseModel
{
    public int OrderId { get; set; }
    public int CustomerDesignId { get; set; }

    // Snapshotted product data at purchase time
    public string ProductName { get; set; } = string.Empty;
    public string ColorName { get; set; } = string.Empty;
    public string SizeName { get; set; } = string.Empty;
    public int Quantity { get; set; }
    public decimal UnitPrice { get; set; }

    public Order Order { get; set; } = null!;
    public CustomerDesign CustomerDesign { get; set; } = null!;
}
