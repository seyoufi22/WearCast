namespace WearCast.Api.Common.Interfaces
{
    public interface INotificationEvent
    {
        List<string> RecipientIds { get; }
        public string Title { get; }
        public string Message { get; }
        public NotificationType NotificationType { get; }
        public int? UrlId { get; }
    }
}