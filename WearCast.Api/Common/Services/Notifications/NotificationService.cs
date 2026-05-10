using Microsoft.AspNetCore.SignalR;
using WearCast.Api.Features.NotificationManagement.NotificationHub;

namespace WearCast.Api.Common.Services.Notifications
{
    public class NotificationService : INotificationService
    {
        private readonly IHubContext<NotificationHub> _hub;
        private readonly ILogger<NotificationService> _logger;

        public NotificationService(
            IHubContext<NotificationHub> hub,
            ILogger<NotificationService> logger)
        {
            _hub = hub;
            _logger = logger;
        }

        public async Task SendNotificationsAsync(List<NotificationDto> notifications)
        {
            try
            {
                var tasks = notifications.Select(notification =>
                    _hub.Clients
                        .User(notification.UserId)
                        .SendAsync("ReceiveNotification", notification));

                await Task.WhenAll(tasks);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to send notifications");
            }
        }
    }
}
