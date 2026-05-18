namespace WearCast.Api.Features.Shipments.Driver.GetShipmentById.DTOs
{
    public class GetDriverShipmentByIdResponseDTO
    {
        public int Id { get; set; }
        public Address DeliveryAddress { get; set; }
        public ShipmentStatus ShipmentStatus { get; set; }
        public DateTime OrderedAt { get; set; }
        public DateTime? ReadyForPickupAt { get; set; }
        public DateTime? TripStartedAt { get; set; }
        public DateTime? OutForDeliveryAt { get; set; }
        public DateTime? DeliveredAt { get; set; }
        public string CustomerName { get; set; }
        public string CustomerPhoneNumber { get; set; }
    }
}
