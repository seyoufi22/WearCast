namespace WearCast.Api.Features.Shipments.AdminAndManager.GetShipmentById.DTOs
{
    public class GetShipmentByIdResponseDTO
    {
        public int Id { get; set; }
        public bool IsDeleted { get; set; }
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
        public List<GetShipmentByIdOrderSummaryDTO> Orders { get; set; }

    }
    public class GetShipmentByIdOrderSummaryDTO
    {
        public int OrderId { get; set; }
        public string StoreName { get; set; }
        public int ItemsCount { get; set; }

        //pick up code
    }
}
