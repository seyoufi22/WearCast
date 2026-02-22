namespace WearCast.Api.Entities.BusinessActors
{
    public class Seller
    {
        public int Id { get; set; }

        public string SellerName { get; set; } = string.Empty;

        public string CommercialRegisterNumber { get; set; } = string.Empty;

        public string TaxIdNumber { get; set; } = string.Empty;

        public string Description { get; set; } = string.Empty;

        public string LogoUrl { get; set; } = string.Empty;


        public string UserId { get; set; }
        public ApplicationUser? ApplicationUser { get; set; }
    }
}
