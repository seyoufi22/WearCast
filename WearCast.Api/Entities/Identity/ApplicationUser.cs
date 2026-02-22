namespace WearCast.Api.Entities.Identity
{
    public sealed class ApplicationUser : IdentityUser
    {
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;

        public Address Address { get; set; } = new Address();

        public string? EmailConfirmationCode { get; set; }
        public DateTime? EmailConfirmationCodeExpiration { get; set; }

        public string? ResetPasswordCode { get; set; }
        public DateTime? ResetPasswordCodeExpiration { get; set; }
        public bool IsDisabled { get; set; }
        public List<RefreshToken> RefreshTokens { get; set; } = [];
    }
}
