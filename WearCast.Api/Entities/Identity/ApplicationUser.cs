namespace WearCast.Api.Entities.Identity
{
    public sealed class ApplicationUser : IdentityUser
    {
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;

        public string? EmailConfirmationCode { get; set; }
        public DateTime? EmailConfirmationCodeExpiration { get; set; }

        public string? ResetPasswordCode { get; set; }
        public DateTime? ResetPasswordCodeExpiration { get; set; }
        public bool IsDisabled { get; set; }
        public List<RefreshToken> RefreshTokens { get; set; } = [];

        public Customer? Customer { get; set; }
        public Driver? Driver { get; set; }
        public SellerManager? SellerManager { get; set; }
        public FactoryManager? FactoryManager { get; set; }
        public ShippingCompanyManager? ShippingCompanyManager { get; set; }
    }
}
