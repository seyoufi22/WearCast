namespace WearCast.Api.Features.Factories.FactoryManagers.GetAllFactoryManagers
{
    [Route("api/factory-managers")]
    [ApiController]
    [Authorize(Roles = $"{DefaultRoles.FactoryManager},{DefaultRoles.SuperAdmin}")]
    [Tags("Factory Manager Profile")]
    public class GetAllFactoryManagersEndpoint(IMediator mediator) : ControllerBase
    {
        private readonly IMediator _mediator = mediator;

        [HttpGet("all")]
        public async Task<IActionResult> GetAllManagers(CancellationToken cancellationToken)
        {
            var result = await _mediator.Send(new GetAllFactoryManagersRequest(), cancellationToken);

            return result.ToResponse();
        }
    }
}