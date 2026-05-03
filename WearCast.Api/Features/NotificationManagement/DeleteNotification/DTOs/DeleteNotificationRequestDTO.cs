namespace WearCast.Api.Features.NotificationManagement.DeleteNotification.DTOs
{
    public class DeleteNotificationRequestDTO: IRequest<Result>
    {
        public int NotificationId {  get; set; }
    }
    public class DeleteNotificationValidator : AbstractValidator<DeleteNotificationRequestDTO>
    {
        public DeleteNotificationValidator()
        {
            RuleFor(x => x.NotificationId)
                .GreaterThan(0)
                .WithMessage("Notification ID must be valid.");
        }
    }
}
