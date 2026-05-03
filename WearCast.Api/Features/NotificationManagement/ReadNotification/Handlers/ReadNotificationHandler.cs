using System.Security.Claims;
using WearCast.Api.Features.NotificationManagement.ReadNotification.DTOs;

namespace WearCast.Api.Features.NotificationManagement.ReadNotification.Handlers
{
    public class ReadNotificationHandler : IRequestHandler<ReadNotificationRequestDTO, Result>
    {
        private readonly ApplicationDbContext _context;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public ReadNotificationHandler(ApplicationDbContext context, IHttpContextAccessor httpContextAccessor)
        {
            _context = context;
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task<Result> Handle(ReadNotificationRequestDTO request, CancellationToken cancellationToken)
        {
            var userId = _httpContextAccessor.HttpContext?.User?.FindFirstValue(ClaimTypes.NameIdentifier);

            if (string.IsNullOrEmpty(userId))
                return Result.Failure(NotificationErrors.UserNotFound);

            int deliveryUpdated = await _context.Notifications
                .Where(n => n.Id == request.NotificationId && n.UserId == userId && !n.IsDelivered)
                .ExecuteUpdateAsync(s => s
                .SetProperty(n => n.IsRead, true)
                .SetProperty(n => n.IsDelivered, true), cancellationToken);

            if (deliveryUpdated > 0)
            {
                await _context.Users
                    .Where(u => u.Id == userId)
                    .ExecuteUpdateAsync(s => s.SetProperty(
                        u => u.UndeliveredNotificationsCount,
                        u => u.UndeliveredNotificationsCount - 1),
                        cancellationToken);
            }
            else
            {
                int readUpdated = await _context.Notifications
                    .Where(n => n.Id == request.NotificationId && n.UserId == userId)
                    .ExecuteUpdateAsync(s => s.SetProperty(n => n.IsRead, true), cancellationToken);

                if (readUpdated == 0)
                    return Result.Failure(NotificationErrors.NotificationNotFound);
            }
            return Result.Success();
        }
    }
}