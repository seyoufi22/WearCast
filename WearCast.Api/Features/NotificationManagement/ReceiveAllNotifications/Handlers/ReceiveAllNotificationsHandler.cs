using System.Security.Claims;
using WearCast.Api.Features.NotificationManagement.ReceiveAllNotifications.DTOs;

namespace WearCast.Api.Features.NotificationManagement.ReceiveAllNotifications.Handlers
{
    public class ReceiveAllNotificationsHandler : IRequestHandler<ReceiveAllNotificationsRequestDTO, Result>
    {
        private readonly ApplicationDbContext _context;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public ReceiveAllNotificationsHandler(ApplicationDbContext context, IHttpContextAccessor httpContextAccessor)
        {
            _context = context;
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task<Result> Handle(ReceiveAllNotificationsRequestDTO request, CancellationToken cancellationToken)
        {
            var userId = _httpContextAccessor.HttpContext?.User?.FindFirstValue(ClaimTypes.NameIdentifier);

            if (string.IsNullOrEmpty(userId))
                return Result.Failure(NotificationErrors.UserNotFound);

            await _context.Notifications
                .Where(n => n.UserId == userId && !n.IsDelivered)
                .ExecuteUpdateAsync(s => s.SetProperty(n => n.IsDelivered, true), cancellationToken);

            await _context.Users
                .Where(u => u.Id == userId)
                .ExecuteUpdateAsync(s => s.SetProperty(u => u.UndeliveredNotificationsCount, 0), cancellationToken);

            return Result.Success();
        }
    }
}
