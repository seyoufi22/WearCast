namespace WearCast.Api.Features.ShippingCompanies.GetAllOrders.DTOs
{
    public class GetAllOrdersResponseDTO
    {
        public int OrderId { get; set; }
        public int ShipmentId { get; set; }
        public OrderType OrderType { get; set; }
        public string VendorName { get; set; } = string.Empty;
        public Address VendorAddress { get; set; } = new Address();
        public string VendorPhoneNumber { get; set; } = string.Empty;
        public OrderStatus OrderStatus { get; set; }
        public int NumberOfItems { get; set; }
        public ShipmentStatus ShipmentStatus { get; set; }
        public DateTime CreatedOn { get; set; }
    }
}
