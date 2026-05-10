using System.Security.Claims;
using WearCast.Api.Features.NotificationManagement.ReadAllNotifications.DTOs;

namespace WearCast.Api.Features.NotificationManagement.ReadAllNotifications.Handlers
{
    public class ReadAllNotificationsHandler : IRequestHandler<ReadAllNotificationsRequestDTO, Result>
    {
        private readonly ApplicationDbContext _context;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public ReadAllNotificationsHandler(ApplicationDbContext context, IHttpContextAccessor httpContextAccessor)
        {
            _context = context;
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task<Result> Handle(ReadAllNotificationsRequestDTO request, CancellationToken cancellationToken)
        {
            var userId = _httpContextAccessor.HttpContext?.User?.FindFirstValue(ClaimTypes.NameIdentifier);

            if (string.IsNullOrEmpty(userId))
                return Result.Failure(NotificationErrors.UserNotFound);

            await _context.Notifications
                .Where(n => n.UserId == userId && !n.IsRead)
                .ExecuteUpdateAsync(s => s.SetProperty(n => n.IsRead, true), cancellationToken);

            await _context.Users
                .Where(u => u.Id == userId)
                .ExecuteUpdateAsync(s => s.SetProperty(u => u.UndeliveredNotificationsCount, 0), cancellationToken);

            return Result.Success();
        }
    }
}