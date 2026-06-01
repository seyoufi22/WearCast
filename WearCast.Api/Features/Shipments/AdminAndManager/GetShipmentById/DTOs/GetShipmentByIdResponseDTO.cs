namespace WearCast.Api.Features.Shipments.AdminAndManager.GetShipmentById.DTOs
{
    public class GetShipmentByIdResponseDTO
    {
        public int Id { get; set; }
        public Address DeliveryAddress { get; set; }
        public decimal Price { get; set; }
        public ShipmentStatus ShipmentStatus { get; set; }
        public DateTime OrderTime { get; set; }
        public DateTime? ReadyForPickupAt { get; set; }
        public DateTime? TripStartedAt { get; set; }
        public DateTime? OutForDeliveryAt { get; set; }
        public DateTime? DeliveredAt { get; set; }
        public string DeliveryCode { get; set; }
        public int? DriverId { get; set; }
        public string? DriverName { get; set; }
        public string? DriverPhoneNumber { get; set; }
        public string? DriverNationalId { get; set; }

        public int CustomerId { get; set; }
        public string CustomerName { get; set; }
        public string CustomerPhoneNumber { get; set; }

    }
}
