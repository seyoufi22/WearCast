namespace WearCast.Api.Features.Factories.FactoryManagers.CreateFactoryManager
{
    [Route("api/factory-managers")]
    [ApiController]
    [Authorize(Roles = $"{DefaultRoles.FactoryManager},{DefaultRoles.VendorAdmin},{DefaultRoles.SuperAdmin}")]
    [Tags("Factory Manager Profile")]
    public class CreateFactoryManagerEndPoint(IMediator mediator) : ControllerBase
    {
        private readonly IMediator _mediator = mediator;

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateFactoryManagerRequest request, CancellationToken cancellationToken)
        {
            var result = await _mediator.Send(request, cancellationToken);

            return result.ToResponse();
        }
    }
}
