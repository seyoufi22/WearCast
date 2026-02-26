namespace WearCast.Api.Entities.Shipping
{
    public class Shipment : BaseModel
    {
        public Address PickUpAddress { get; set; }
        public Address DeliveryAddress { get; set; }
        public decimal Price { get; set; }
        public int? DriverId { get; set; }
        public Driver? Driver { get; set; } = default;

        public int? ShippingCompanyId { get; set; }
        public ShippingCompany? ShippingCompany { get; set; } = default!;

        public int? CustomerID {get;set;}
        public Customer? Customer { get; set; } = default;

    }
}
