using WearCast.Api.Common.Views;

namespace WearCast.Api.Features.NotificationManagement.GetAllNotifications.DTOs
{
    public class GetAllNotificationsRequestDTO : IRequest<Result<PagingViewModel<GetAllNotificationsResponseDTO>>>
    {
        public int PageIndex { get; set; } = 1;
        public int PageSize { get; set; } = 100;

        public SortBy SortBy { get; set; } = SortBy.Newest;

        public bool? IsRead { get; set; } = null;
        public NotificationType? NotificationType { get; set; } = null;

    }
    public class GetAllNotificationsValidator : AbstractValidator<GetAllNotificationsRequestDTO>
    {
        public GetAllNotificationsValidator()
        {
            RuleFor(x => x.PageIndex)
                .GreaterThan(0)
                .WithMessage("Page index must be greater than 0.");

            RuleFor(x => x.PageSize)
                .InclusiveBetween(1, 100)
                .WithMessage("Page size must be between 1 and 100.");

            RuleFor(x => x.SortBy)
                .IsInEnum()
                .WithMessage("Invalid sort option.");

            RuleFor(x => x.NotificationType)
               .IsInEnum()
               .When(x => x.NotificationType.HasValue)
               .WithMessage("Invalid notification type value.");

        }
    }
}
