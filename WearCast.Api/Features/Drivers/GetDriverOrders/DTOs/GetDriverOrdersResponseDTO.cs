namespace WearCast.Api.Features.Drivers.GetDriverOrders.DTOs
{
    public class GetDriverOrdersResponseDTO
    {
        public int OrderId { get; set; }
        public int ShipmentId {  get; set; }
        public OrderType OrderType { get; set; }
        public string VendorName { get; set; }
        public Address VendorAddress { get; set; }
        public string VendorPhoneNumber { get; set; }
        public OrderStatus OrderStatus { get; set; }
        public int NumberOfItems { get; set; }
    }
}
