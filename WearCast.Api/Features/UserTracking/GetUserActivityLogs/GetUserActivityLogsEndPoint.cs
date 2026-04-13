namespace WearCast.Api.Features.UserTracking.GetUserActivityLogs
{
    [Route("api/tracking")]
    [ApiController]
    [Tags("Tracking")]
    public class GetUserActivityLogsEndPoint(IMediator mediator) : ControllerBase
    {
        private readonly IMediator _mediator = mediator;
        [HttpGet("users/{userId}/logs")]
        public async Task<IActionResult> GetLogs([FromRoute] string userId, CancellationToken cancellationToken)
        {
            var result = await _mediator.Send(new GetUserActivityLogsRequest(userId), cancellationToken);

            return result.ToResponse();
        }
    }
}
