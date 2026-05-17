namespace WearCast.Api.Features.Drivers.GetDriverById.DTOs
{
    public class GetDriverByIdResponseDTO
    {
        public int Id { get; set; }
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string PhoneNumber { get; set; } = string.Empty;
        public string ProfileImageUrl { get; set; } = string.Empty;
        public string NationalId { get; set; } = string.Empty;
        public DeliveryVehicleType VehicleType { get; set; }
        public string? VehiclePlateNumber { get; set; }
        public DriverStatus Status { get; set; }
        public Address Address { get; set; }
        public bool IsDeleted { get; set; }
        public int NumberOfAssignedShipments{ get; set; }
        public int NumberOfActiveShipments{ get; set; }
        public int NumberOfDeliveredShipments{ get; set; }

    }
}
