using System.Security.Claims;
using WearCast.Api.Common.Helper;
using WearCast.Api.Common.Views;
using WearCast.Api.Features.NotificationManagement.GetAllNotifications.DTOs;

namespace WearCast.Api.Features.NotificationManagement.GetAllNotifications.Handlers
{
    public class GetAllNotificationsHandler : IRequestHandler<GetAllNotificationsRequestDTO, Result<PagingViewModel<GetAllNotificationsResponseDTO>>>
    {
        private readonly ApplicationDbContext _context;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public GetAllNotificationsHandler(ApplicationDbContext context, IHttpContextAccessor httpContextAccessor)
        {
            _context = context;
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task<Result<PagingViewModel<GetAllNotificationsResponseDTO>>> Handle(
            GetAllNotificationsRequestDTO request,
            CancellationToken cancellationToken)
        {
            var query = _context.Notifications.AsNoTracking();
            var userId = _httpContextAccessor.HttpContext?.User?.FindFirstValue(ClaimTypes.NameIdentifier);

            if (string.IsNullOrEmpty(userId))
                return Result.Failure<PagingViewModel<GetAllNotificationsResponseDTO>>(NotificationErrors.UserNotFound);

            query = query.Where(n => n.UserId == userId);
            if (request.NotificationType.HasValue)
            {
                query = query.Where(n => n.NotificationType == request.NotificationType);
            }

            if (request.IsRead.HasValue)
            {
                query = query.Where(n => n.IsRead == request.IsRead);
            }

            query = request.SortBy switch
            {
                SortBy.Oldest => query.OrderBy(s => s.CreatedOn),
                _ => query.OrderByDescending(s => s.CreatedOn)
            };

            var notificationsquery = query
                .Select(n => new GetAllNotificationsResponseDTO
                {
                    Id = n.Id,
                    NotificationType = n.NotificationType,
                    Title = n.Title,
                    Message = n.Message,
                    IsRead = n.IsRead,
                    UrlId = n.UrlId,
                    CreatedOn = n.CreatedOn,

                });
            var pagedResult = await PagingHelper.CreateAsync(notificationsquery, request.PageIndex, request.PageSize);

            return Result.Success(pagedResult);
        }
    }
}