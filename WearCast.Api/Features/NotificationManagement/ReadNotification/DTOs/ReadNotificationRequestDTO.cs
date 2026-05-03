using WearCast.Api.Features.NotificationManagement.GetAllNotifications.DTOs;

namespace WearCast.Api.Features.NotificationManagement.ReadNotification.DTOs
{
    public class ReadNotificationRequestDTO : IRequest<Result>
    {
        public int NotificationId { get; set; }
    }
    public class ReadNotificationValidator : AbstractValidator<ReadNotificationRequestDTO>
    {
        public ReadNotificationValidator()
        {
            RuleFor(x => x.NotificationId)
               .GreaterThan(0)
               .WithMessage("Notification id must be valid.");
        }
    }
}