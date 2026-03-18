namespace WearCast.Api.Features.Shipments.GetShipmentById.DTOs
{
    public class GetShipmentByIdResponseDTO
    {
        public int Id { get; set; }
        public DateTime OrderTime { get; set; } 
        public Address PickUpAddress { get; set; }
        public Address DeliveryAddress { get; set; }
        public decimal Price { get; set; }
        public ShipmentStatus ShipmentStatus { get; set; }
        public int CustomerID { get; set; }
        public string CustomerName { get; set; }
        public string CustomerPhoneNumber { get; set; }
        public int? DriverId { get; set; }
        public string? DriverName { get; set; }
        public string? DriverPhoneNumber { get; set; }
    }
}
