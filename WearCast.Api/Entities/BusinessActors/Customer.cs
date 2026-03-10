namespace WearCast.Api.Entities.BusinessActors
{
    public class Customer
    {
        public int Id { get; set; }

        public Address Address { get; set; } = new Address();

        public string? ProfileImageUrl { get; set; }
        public string UserId { get; set; }
        public ApplicationUser? ApplicationUser { get; set; }

        public ICollection<CustomerDesign> Designs { get; set; } = [];

        public ICollection<CartItem> CartItems { get; set; } = new List<CartItem>();

    }
}
