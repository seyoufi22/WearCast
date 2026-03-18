    namespace WearCast.Api.Features.Shipments.GetAllShipments.DTOs
{
    public class GetAllShipmentsResponseDTO
    {
        public int Id { get; set; }
        public DateTime OrderTime { get; set; }
        public decimal Price { get; set; }
        public ShipmentStatus ShipmentStatus { get; set; }
        public string CustomerName { get; set; }
        public string CustomerPhoneNumber { get; set; }
        public string? DriverName { get; set; } 
        public string PickUpCity { get; set; }
        public string DeliveryCity { get; set; }
    }
}
