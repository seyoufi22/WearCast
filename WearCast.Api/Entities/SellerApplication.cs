

namespace WearCast.Api.Entities
{
    public class SellerApplication
    {
        public int Id { get; set; }


        public string ManagerFirstName { get; set; } = string.Empty;
        public string ManagerLastName { get; set; } = string.Empty;
        public string ManagerEmail { get; set; } = string.Empty;
        public string ManagerPhoneNumber { get; set; } = string.Empty;
        public string ManagerPasswordHash { get; set; } = string.Empty;


        public string SellerName { get; set; } = string.Empty;
        public string SellerEmail { get; set; } = string.Empty;
        public string SellerPhoneNumber { get; set; } = string.Empty;
        public string CommercialRegisterNumber { get; set; } = string.Empty;
        public string TaxIdNumber { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public string LogoUrl { get; set; } = string.Empty;
        public Address SellerAddress { get; set; } = new Address();


        public string? ManagerEmailConfirmationCode { get; set; }
        public DateTime? ManagerEmailConfirmationCodeExpiration { get; set; }
        public bool ManagerEmailConfirmed { get; set; }

        public DateTime CreatedOn { get; set; } = DateTime.UtcNow;
        public Status Status { get; set; } = Status.Pending;
        public string? RejectionReason { get; set; }
    }
}
