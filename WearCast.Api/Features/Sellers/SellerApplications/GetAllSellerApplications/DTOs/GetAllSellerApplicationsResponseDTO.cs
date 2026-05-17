namespace WearCast.Api.Features.Sellers.SellerApplications.GetAllSellerApplications.DTOs
{
    public class GetAllSellerApplicationsResponseDTO
    {
        public int Id { get; set; }
        public string SellerName { get; set; } = string.Empty;
        public string ManagerName { get; set; } = string.Empty;
        public string City { get; set; } = string.Empty;
        public Status Status { get; set; }
        public bool IsEmailConfirmed { get; set; }
        public string SellerEmail {  get; set; }
        public DateTime CreatedOn { get; set; }
    }
}
