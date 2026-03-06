namespace WearCast.Api.Entities.BusinessActors
{
    public class SellerManager
    {
        public int Id { get; set; }


        public string UserId { get; set; }
        public ApplicationUser? ApplicationUser { get; set; }

        public int SellerId { get; set; }
        public Seller? Seller { get; set; }
    }
}
