namespace WearCast.Api.Features.Drivers.GetAllDrivers.DTOs
{
    public class GetAllDriversResponseDTO
    {
        public int Id { get; set; }
        public string DriverName { get; set; } = string.Empty;
        public DeliveryVehicleType VehicleType { get; set; }
        public string? VehiclePlateNumber { get; set; }
        public int ShipmentsCount { get; set; }
        public DriverStatus Status { get; set; }
    }
}
