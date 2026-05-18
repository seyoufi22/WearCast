namespace WearCast.Api.Features.Shipments.Customer.GetShipmentById.DTOs
{
    public class GetCustomerShipmentByIdResponseDTO
    {
        public int Id { get; set; }
        public Address DeliveryAddress { get; set; }
        public decimal Price { get; set; }
        public ShipmentStatus ShipmentStatus { get; set; }
        public DateTime OrderedAt { get; set; }
        public DateTime? ReadyForPickupAt { get; set; }
        public DateTime? TripStartedAt { get; set; }
        public DateTime? OutForDeliveryAt { get; set; }
        public DateTime? DeliveredAt { get; set; }
        public string? DriverName { get; set; }
        public string? DriverPhoneNumber { get; set; }
        public string DeliveryCode { get; set; }

    }
}
