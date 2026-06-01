namespace WearCast.Api.Features.Shipments.AdminAndManager.GetAllShipments.DTOs
{
    public class GetAllShipmentsResponseDTO
    {
        public int Id { get; set; }
        public DateTime OrderTime { get; set; }
        public ShipmentStatus ShipmentStatus { get; set; }
        public decimal Price { get; set; }
        public int NumberOfOrders { get; set; }
        public string DeliveryState { get; set; }
        public string DeliveryCity { get; set; }
        public string DeliveryStreet { get; set; }
        public string DeliveryCode { get; set; }
        public string? DriverName { get; set; }
        public string CustomerName { get; set; }

    }
}
