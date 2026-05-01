namespace WearCast.Api.Entities.BusinessActors
{
    public class ShippingCompanyManager : ISoftDeletable
    {
        public int Id { get; set; }

        public bool IsDeleted { get; set; }

        public string UserId { get; set; }
        public ApplicationUser ApplicationUser { get; set; }

        public int ShippingCompanyId { get; set; }
        public ShippingCompany? ShippingCompany { get; set; }
    }
}
