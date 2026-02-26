using WearCast.Api.Entities.Identity;
using WearCast.Api.Entities.Shipping;

namespace WearCast.Api.Entities.BusinessActors
{
    public class ShippingCompany
    {
        public int Id { get; set; }

        public string UserId { get; set; }
        public ApplicationUser? ApplicationUser { get; set; }
        public List<Driver> Drivers { get; set; } = new List<Driver>();
        public List<Shipment> Shipments { get; set; } = new List<Shipment>();
    }
}
