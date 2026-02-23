namespace WearCast.Api.Features.AuthenticationManagement.Register
{
    [Route("api/auth")]
    [ApiController]
    public class RegisterCustomerEndPoint(IMediator mediator) : ControllerBase
    {
        private readonly IMediator _mediator = mediator;

        [HttpPost("register-customer")]
        public async Task<IActionResult> Register([FromForm] RegisterCustomerRequest request, CancellationToken cancellationToken)
        {
            var result = await _mediator.Send(request, cancellationToken);

            return result.ToResponse();
        }

    }
}
