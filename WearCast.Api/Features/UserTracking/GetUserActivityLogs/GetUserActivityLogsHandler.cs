namespace WearCast.Api.Features.UserTracking.GetUserActivityLogs
{
    public class GetUserActivityLogsHandler(
        ApplicationDbContext context
        ) : IRequestHandler<GetUserActivityLogsRequest, Result<IEnumerable<GetUserActivityLogsResponse>>>
    {
        private readonly ApplicationDbContext _context = context;

        public async Task<Result<IEnumerable<GetUserActivityLogsResponse>>> Handle(GetUserActivityLogsRequest request, CancellationToken cancellationToken)
        {
            var sixMonthsAgo = DateTime.UtcNow.AddMonths(-6);

            var logs = await _context.UserActivityLogs
                .AsNoTracking()
                .Where(log =>
                    log.UserId == request.UserId &&
                    log.CreatedAt >= sixMonthsAgo)
                .OrderByDescending(log => log.CreatedAt)
                .Select(log => new GetUserActivityLogsResponse(
                    log.Payload
                ))
                .ToListAsync(cancellationToken);

            return Result.Success<IEnumerable<GetUserActivityLogsResponse>>(logs);
        }
    }
}
