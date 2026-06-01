namespace WearCast.Api.Common.Services.Notifications
{
    public class NotificationDto
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Message { get; set; }
        public bool IsRead { get; set; } = false;
        public bool IsDelivered { get; set; } = false;
        public int IncrementCount { get; set; } = 1;
        public DateTime CreatedOn { get; set; } = DateTime.UtcNow;
        public string UserId { get; set; }
        public NotificationType NotificationType { get; set; }
        public int? UrlId { get; set; }

    }
}
