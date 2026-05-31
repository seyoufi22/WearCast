namespace WearCast.Api.Features.Factories.FactoryDashboard
{
    [Route("api/factories/dashboard")]
    [ApiController]
    [Authorize(Roles = DefaultRoles.FactoryManager)]
    [Tags("Factory Dashboard")]
    public class GetFactoryDashboardMetricsEndPoint(IMediator mediator) : ControllerBase
    {
        private readonly IMediator _mediator = mediator;

        [HttpGet("metrics")]
        public async Task<IActionResult> GetMetrics(CancellationToken cancellationToken)
        {
            var result = await _mediator.Send(new GetFactoryDashboardMetricsRequest(), cancellationToken);

            return result.ToResponse();
        }
    }
}
