namespace WearCast.Api.Common.Interfaces
{
    public interface INotificationService
    {
        Task SendNotificationsAsync(List<NotificationDto> notifications);
    }
}
