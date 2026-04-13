namespace WearCast.Api.Features.UserTracking.GetUserActivityLogs
{
    public class GetUserActivityLogsRequestValidator : AbstractValidator<GetUserActivityLogsRequest>
    {
        public GetUserActivityLogsRequestValidator()
        {
            RuleFor(x => x.UserId)
                .NotEmpty();
        }
    }
}
