namespace WearCast.Api.Common.Interfaces.Notifications
{
    public interface INotificationEvent
    {
        public List<string> RecipientIds { get; }
        public string Title { get; }
        public string Message { get; }
        public NotificationType NotificationType { get; }
        public int? UrlId { get; }
    }
}