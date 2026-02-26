using WearCast.Api.Entities.Shipping;

namespace WearCast.Api.Entities.BusinessActors
{
    public class Customer
    {
        public int Id { get; set; }
        public string? ProfileImageUrl { get; set; }
        public string UserId { get; set; }
        public ApplicationUser? ApplicationUser { get; set; }
        public List<Shipment> Shipments { get; set; }=new List<Shipment>();
    }
}
