namespace WearCast.Api.Features.Drivers.GetAllDrivers.DTOs
{
    public class GetAllDriversResponseDTO
    {
        public int Id { get; set; }
        public string DriverName { get; set; } = string.Empty;
        public DeliveryVehicleType VehicleType { get; set; }
        public DriverStatus Status { get; set; }
        public string DriverCity {  get; set; }
        public int NumberOfAssignedShipments { get; set; }
        public int NumberOfActiveShipments { get; set; }
        public int NumberOfDeliveredShipments { get; set; }
    }
}
