namespace WearCast.Api.Entities.BusinessActors
{
    public class Seller : ISoftDeletable
    {
        public int Id { get; set; }

        public string Name { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string PhoneNumber { get; set; } = string.Empty;

        public string CommercialRegisterNumber { get; set; } = string.Empty;

        public string TaxIdNumber { get; set; } = string.Empty;

        public string Description { get; set; } = string.Empty;

        public string LogoUrl { get; set; } = string.Empty;

        public Address Address { get; set; } = new Address();

        public bool IsDeleted { get; set; }

        public ICollection<SellerManager> Managers { get; set; } = new List<SellerManager>();

        public ICollection<FixedProduct.FixedProduct> FixedProducts { get; set; } = new List<FixedProduct.FixedProduct>();
    }
}
