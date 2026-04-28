using WearCast.Api.Entities.BusinessActors;

namespace WearCast.Api.Entities.Order;

public class Order : BaseModel
{
    public int CustomerId { get; set; }
    public int? SellerId { get; set; }
    public int? FactoryId { get; set; }
    public decimal TotalAmount { get; set; }
    public OrderStatus Status { get; set; } = OrderStatus.Pending;
    public string? StripeSessionId { get; set; }
    public string? StripePaymentIntentId { get; set; }

    // Shipping / recipient info
    public string RecipientName { get; set; } = string.Empty;
    public string RecipientPhoneNumber { get; set; } = string.Empty;
    public string? RecipientAdditionalPhoneNumber { get; set; }
    public Address ShippingAddress { get; set; } = new();
    public Address PickUpAddress { get; set; } = new();

    public Customer Customer { get; set; } = null!;
    public Seller? Seller { get; set; }
    public Factory? Factory { get; set; }
    public ICollection<FixedProductOrderItem> FixedProductItems { get; set; } = new List<FixedProductOrderItem>();
    public ICollection<CustomerDesignedOrderItem> DesignedProductItems { get; set; } = new List<CustomerDesignedOrderItem>();

    public int? ShipmentId { get; set; }
    public Shipping.Shipment? Shipment { get; set; }
}
