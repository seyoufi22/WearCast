

namespace WearCast.Api.Entities
{
    public class SellerApplication
    {
        public int Id { get; set; }
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string PhoneNumber { get; set; } = string.Empty;

        public string? EmailConfirmationCode { get; set; }
        public DateTime? EmailConfirmationCodeExpiration { get; set; }

        public bool EmailConfirmed { get; set; }

        public string PasswordHash { get; set; } = string.Empty;

        public string SellerName { get; set; } = string.Empty;
        public string CommercialRegisterNumber { get; set; } = string.Empty;
        public string TaxIdNumber { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public string LogoUrl { get; set; } = string.Empty;
        public Address StoreAddress { get; set; } = new Address();

        public DateTime CreatedOn { get; set; } = DateTime.UtcNow;

        public Status Status { get; set; } = Status.Pending;
        public string? RejectionReason { get; set; }
    }
}
