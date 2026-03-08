namespace WearCast.Api.Entities.BusinessActors
{
    public class ShippingCompanyManager
    {
        public int Id { get; set; }

        public string UserId { get; set; }
        public ApplicationUser ApplicationUser { get; set; }

        public int ShippingCompanyId { get; set; }
        public ShippingCompany? ShippingCompany { get; set; }
    }
}
