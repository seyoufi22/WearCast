namespace WearCast.Api.Features.UserTracking.GetUserActivityLogs
{
    public record GetUserActivityLogsRequest(string UserId)
        : IRequest<Result<IEnumerable<GetUserActivityLogsResponse>>>;
}
