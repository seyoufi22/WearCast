using System.Security.Claims;
using WearCast.Api.Features.NotificationManagement.DeleteNotification.DTOs;

namespace WearCast.Api.Features.NotificationManagement.DeleteNotification.Handlers
{
    public class DeleteNotificationHandler: IRequestHandler<DeleteNotificationRequestDTO,Result>
    {
        private readonly ApplicationDbContext _context;
        private readonly IHttpContextAccessor _httpContextAccessor;
        public DeleteNotificationHandler(ApplicationDbContext context, IHttpContextAccessor httpContextAccessor)
        {
            _context = context;
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task<Result> Handle(DeleteNotificationRequestDTO request, CancellationToken cancellationToken)
        {
            var userId = _httpContextAccessor.HttpContext?.User?.FindFirstValue(ClaimTypes.NameIdentifier);
            var notification = await _context.Notifications
                .FirstOrDefaultAsync(n => n.Id == request.NotificationId && n.UserId == userId, cancellationToken);

            if (notification == null)
                return Result.Failure(NotificationErrors.NotFound);

            notification.IsDeleted = true;

            await _context.SaveChangesAsync(cancellationToken);

            return Result.Success();
        }
    }
}
