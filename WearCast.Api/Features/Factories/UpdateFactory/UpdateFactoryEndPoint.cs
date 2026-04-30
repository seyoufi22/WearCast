namespace WearCast.Api.Features.Factories.UpdateFactory
{
    [Route("api/factories")]
    [ApiController]
    [Authorize(Roles = $"{DefaultRoles.FactoryManager},{DefaultRoles.VendorAdmin},{DefaultRoles.SuperAdmin}")]
    [Tags("Factory Profile")]
    public class UpdateFactoryEndPoint(IMediator mediator) : ControllerBase
    {
        private readonly IMediator _mediator = mediator;
        [HttpPut("profile")]
        public async Task<IActionResult> UpdateProfile([FromBody] UpdateFactoryRequest request, CancellationToken cancellationToken)
        {
            var result = await _mediator.Send(request, cancellationToken);

            return result.ToResponse();
        }
    }
}
