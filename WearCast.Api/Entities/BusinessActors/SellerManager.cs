namespace WearCast.Api.Entities.BusinessActors
{
    public class SellerManager : ISoftDeletable
    {
        public int Id { get; set; }

        public bool IsDeleted { get; set; }

        public string UserId { get; set; }
        public ApplicationUser? ApplicationUser { get; set; }

        public int SellerId { get; set; }
        public Seller? Seller { get; set; }
    }
}
