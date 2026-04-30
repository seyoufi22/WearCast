namespace WearCast.Api.Features.Factories.FactoryManagers.UpdateFactoryManager
{
    [Route("api/factory-managers")]
    [ApiController]
    [Authorize(Roles = $"{DefaultRoles.FactoryManager},{DefaultRoles.VendorAdmin},{DefaultRoles.SuperAdmin}")]
    [Tags("Factory Manager Profile")]
    public class UpdateFactoryManagerEndPoint(IMediator mediator) : ControllerBase
    {
        private readonly IMediator _mediator = mediator;

        [HttpPut("profile")]
        public async Task<IActionResult> UpdateProfile([FromBody] UpdateFactoryManagerRequest request, CancellationToken cancellationToken)
        {
            var result = await _mediator.Send(request, cancellationToken);

            return result.ToResponse();
        }
    }
}
