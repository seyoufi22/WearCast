namespace WearCast.Api.Features.NotificationManagement.GetAllNotifications.DTOs
{
    public class GetAllNotificationsResponseDTO
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Message { get; set; }
        public bool IsRead { get; set; } = false;
        public DateTime CreatedOn { get; set; } = DateTime.UtcNow;
        public NotificationType NotificationType { get; set; }
        public int? UrlId { get; set; }
    }
}
