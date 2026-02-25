using WearCast.Api.Entities.Identity;

namespace WearCast.Api.Entities.BusinessActors
{
    public class ShippingCompany
    {
        public int Id { get; set; }


        public string UserId { get; set; }
        public ApplicationUser? ApplicationUser { get; set; }
    }
}
