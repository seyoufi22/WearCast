namespace WearCast.Api.Features.Drivers.UpdateDriver
{
    [Route("api/drivers")]
    [ApiController]
    [Authorize(Roles = $"{DefaultRoles.Driver},{DefaultRoles.ShippingCompanyManager},{DefaultRoles.SuperAdmin}")]
    [Tags("Driver Profile")]
    public class UpdateDriverProfileEndPoint(IMediator mediator) : ControllerBase
    {
        private readonly IMediator _mediator = mediator;

        [HttpPut("profile")]
        public async Task<IActionResult> Update([FromBody] UpdateDriverRequest request, CancellationToken cancellationToken)
        {
            var result = await _mediator.Send(request, cancellationToken);

            return result.ToResponse();
        }
    }
}
