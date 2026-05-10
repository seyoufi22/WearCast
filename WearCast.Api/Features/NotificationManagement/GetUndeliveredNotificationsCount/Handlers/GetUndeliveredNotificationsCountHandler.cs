using System.Security.Claims;
using WearCast.Api.Features.AuthenticationManagement;
using WearCast.Api.Features.NotificationManagement.GetUndeliveredNotificationsCount.DTOs;

namespace WearCast.Api.Features.NotificationManagement.GetUndeliveredNotificationsCount.Handlers
{
    public class GetUndeliveredNotificationsCountHandler : IRequestHandler<GetUndeliveredNotificationsCountRequestDTO, Result<int>>
    {
        private readonly ApplicationDbContext _context;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public GetUndeliveredNotificationsCountHandler(ApplicationDbContext context, IHttpContextAccessor httpContextAccessor)
        {
            _context = context;
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task<Result<int>> Handle(GetUndeliveredNotificationsCountRequestDTO request, CancellationToken cancellationToken)
        {
            var userId = _httpContextAccessor.HttpContext?.User?.FindFirstValue(ClaimTypes.NameIdentifier);

            if (string.IsNullOrEmpty(userId))
                return Result.Failure<int>(NotificationErrors.UserNotFound);

            var count = await _context.Users
                .AsNoTracking()
                .Where(u => u.Id == userId)
                .Select(u => u.UndeliveredNotificationsCount)
                .FirstOrDefaultAsync(cancellationToken);

            return Result.Success(count);
        }
    }
}
