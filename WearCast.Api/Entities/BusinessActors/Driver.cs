using WearCast.Api.Entities.Shipping;
namespace WearCast.Api.Entities.BusinessActors
{
    public class Driver
    {
        public int Id { get; set; }
        public string UserId { get; set; }
        public ApplicationUser? ApplicationUser { get; set; }
        public bool IsAvailable { get; set; } = true;
        public List<Shipment> Shipments { get; set; } = new List<Shipment>();
        public int ShippingCompanyId { get; set; }
        public ShippingCompany ShippingCompany { get; set; } = default!;

    }
}
