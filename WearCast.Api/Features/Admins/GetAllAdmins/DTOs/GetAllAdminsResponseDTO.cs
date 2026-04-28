namespace WearCast.Api.Features.Admins.GetAllAdmins.DTOs
{
    public class GetAllAdminsResponseDTO
    {
        public string Id { get; set; } = string.Empty;
        public string FullName { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string PhoneNumber { get; set; }= string.Empty;
        public bool IsDeleted { get; set; }
        public string Role { get; set; } = string.Empty;
    }
}
