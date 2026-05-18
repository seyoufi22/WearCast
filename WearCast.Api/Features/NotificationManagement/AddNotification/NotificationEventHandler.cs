using Hangfire;
using WearCast.Api.Common.Interfaces.Notifications;
using WearCast.Api.Common.Services.Notifications;

namespace WearCast.Api.Features.NotificationManagement.AddNotification
{
    public class NotificationEventHandler<TEvent> : INotificationHandler<TEvent>
        where TEvent : INotification, INotificationEvent
    {
        private readonly ApplicationDbContext _context;
        private readonly INotificationService _notificationService;

        public NotificationEventHandler(ApplicationDbContext context,
            INotificationService notificationService)
        {
            _context = context;
            _notificationService = notificationService;
        }
        public async Task Handle(TEvent notification, CancellationToken cancellationToken)
        {
            var notifications = notification.RecipientIds.Select(userId => new Notification
            {
                UserId = userId,
                Message = notification.Message,
                Title = notification.Title,
                NotificationType = notification.NotificationType,
                UrlId = notification.UrlId
            }).ToList();

            await _context.Notifications.AddRangeAsync(notifications);
            await _context.SaveChangesAsync();

            await _context.Users
                .Where(u => notification.RecipientIds.Contains(u.Id))
                .ExecuteUpdateAsync(s => s.SetProperty(
                    u => u.UndeliveredNotificationsCount,
                    u => u.UndeliveredNotificationsCount + 1),
                cancellationToken);
            var notificationDtos = notifications.Select(n => new NotificationDto
            {
                Id = n.Id,
                Title = n.Title,
                Message = n.Message,
                IsRead = n.IsRead,
                IsDelivered = n.IsDelivered,
                IncrementCount = 1,
                CreatedOn = n.CreatedOn,
                UserId = n.UserId,
                NotificationType = n.NotificationType,
                UrlId = n.UrlId
            }).ToList();

            BackgroundJob.Enqueue(() => _notificationService.SendNotificationsAsync(notificationDtos));
        }
    }
}
