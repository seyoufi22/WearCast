namespace WearCast.Api.Entities.Shipping
{
    public class Shipment : BaseModel
    {
        public Address DeliveryAddress { get; set; }
        public DateTime? ReadyForPickupAt { get; set; }
        public DateTime? TripStartedAt { get; set; }
        public DateTime? OutForDeliveryAt { get; set; }
        public DateTime? DeliveredAt { get; set; }
        public string? DeliveryCode { get; set; }
        public decimal Price { get; set; }
        public int? DriverId { get; set; }
        public Driver? Driver { get; set; } = default;

        public ShipmentStatus ShipmentStatus { get; set; } = ShipmentStatus.Pending;
        public int ShippingCompanyId { get; set; }
        public ShippingCompany ShippingCompany { get; set; }

        public int CustomerId { get; set; }
        public Customer Customer { get; set; }

        public ICollection<Order.Order> Orders { get; set; } = new List<Order.Order>();

    }
}
