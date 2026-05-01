namespace WearCast.Api.Features.Shipments.Driver.GetAllShipments.DTOs
{
    public class GetAllDriverShipmentsResponseDTO
    {
        public int Id { get; set; }
        public DateTime OrderTime { get; set; }
        public ShipmentStatus ShipmentStatus { get; set; }
        public string CustomerName { get; set; }
        public string CustomerPhoneNumber { get; set; }
        public int NumberOfOrders { get; set; }
        public string DeliveryCity { get; set; }
        public string DeliveryStreet { get; set; }
    }
}
