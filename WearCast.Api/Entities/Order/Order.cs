using WearCast.Api.Entities.BusinessActors;

namespace WearCast.Api.Entities.Order;

public class Order : BaseModel
{
    public int CustomerId { get; set; }
    public int SellerId { get; set; }
    public decimal TotalAmount { get; set; }
    public OrderStatus Status { get; set; } = OrderStatus.Pending;
    public string? StripeSessionId { get; set; }
    public string? StripePaymentIntentId { get; set; }
    public Address ShippingAddress { get; set; } = new();

    public Customer Customer { get; set; } = null!;
    public Seller Seller { get; set; } = null!;
    public ICollection<FixedProductOrderItem> FixedProductItems { get; set; } = new List<FixedProductOrderItem>();
}
