namespace WearCast.Api.Entities.Shipping
{
    public class Shipment : BaseModel
    {
        public Address DeliveryAddress { get; set; }
        public decimal Price { get; set; }
        public int? DriverId { get; set; }
        public Driver? Driver { get; set; } = default;

        public ShipmentStatus ShipmentStatus { get; set; } = ShipmentStatus.UnAssigned;
        public int ShippingCompanyId { get; set; }
        public ShippingCompany ShippingCompany { get; set; } = default!;

        public int CustomerID {get;set;}
        public Customer Customer { get; set; } = default;

        public ICollection<Order.Order> Orders { get; set; } = new List<Order.Order>();

    }
}
