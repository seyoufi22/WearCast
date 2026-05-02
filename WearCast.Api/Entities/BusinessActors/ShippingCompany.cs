using WearCast.Api.Entities.Identity;
using WearCast.Api.Entities.Shipping;

namespace WearCast.Api.Entities.BusinessActors
{
    public class ShippingCompany : ISoftDeletable
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string PhoneNumber { get; set; } = string.Empty;

        public string CommercialRegisterNumber { get; set; } = string.Empty;

        public string TaxIdNumber { get; set; } = string.Empty;

        public string Description { get; set; } = string.Empty;

        public string LogoUrl { get; set; } = string.Empty;

        public decimal DeliveryFee { get; set; }

        public Address Address { get; set; } = new Address();

        public bool IsDeleted { get; set; }

        public int? WalletId { get; set; }
        public Wallet.Wallet? Wallet { get; set; }

        public ICollection<ShippingCompanyManager> Managers { get; set; } = new List<ShippingCompanyManager>();
        public ICollection<Driver> Drivers { get; set; } = new List<Driver>();
        public List<Shipment> Shipments { get; set; } = new List<Shipment>();

    }
}
