namespace WearCast.Api.Features.AuthenticationManagement.Register
{
    [Route("auth")]
    [ApiController]
    public class RegisterCustomerEndPoint(IMediator mediator) : ControllerBase
    {
        private readonly IMediator _mediator = mediator;

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterCustomerRequest request, CancellationToken cancellationToken)
        {
            var result = await _mediator.Send(request, cancellationToken);

            return result.ToResponse();
        }

    }
}
