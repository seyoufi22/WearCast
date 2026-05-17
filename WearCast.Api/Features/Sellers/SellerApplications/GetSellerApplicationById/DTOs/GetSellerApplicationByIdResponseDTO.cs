namespace WearCast.Api.Features.Sellers.SellerApplications.GetSellerApplicationById.DTOs
{
    public class GetSellerApplicationByIdResponseDTO
    {
        public int Id { get; set; }

        public string ManagerFirstName { get; set; }
        public string ManagerLastName { get; set; }
        public string ManagerEmail { get; set; }
        public string ManagerPhoneNumber { get; set; }
        public bool ManagerEmailConfirmed { get; set; }

        public string SellerName { get; set; }
        public string SellerEmail { get; set; }
        public string SellerPhoneNumber { get; set; }
        public string CommercialRegisterNumber { get; set; }
        public string TaxIdNumber { get; set; }
        public string Description { get; set; }
        public string LogoUrl { get; set; }
        public Address SellerAddress { get; set; }

        public DateTime CreatedOn { get; set; }
        public Status Status { get; set; }
        public string? RejectionReason { get; set; }
    }
}
