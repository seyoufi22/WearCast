namespace WearCast.Api.Entities.BusinessActors
{
    public class Driver : ISoftDeletable
    {
        public int Id { get; set; }

        public DriverStatus Status { get; set; } = DriverStatus.NotAvailable;

        public string ProfileImageUrl { get; set; } = string.Empty;

        public Address Address { get; set; } = new Address();

        public string NationalId { get; set; } = string.Empty;
        public DeliveryVehicleType VehicleType { get; set; }
        public string? VehiclePlateNumber { get; set; }
        public bool IsDeleted { get; set; }

        public string UserId { get; set; }
        public ApplicationUser? ApplicationUser { get; set; }

        public int ShippingCompanyId { get; set; }

        public ShippingCompany? ShippingCompany { get; set; }
    }
}
