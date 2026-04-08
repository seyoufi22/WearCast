namespace WearCast.Api.Features.AuthenticationManagement.Login
{
    [Route("api/auth")]
    [ApiController]
    [Tags("Auth")]
    public class LoginEndPoint(IMediator mediator) : ControllerBase
    {
        private readonly IMediator _mediator = mediator;

        [HttpPost("login")]
        public async Task<IActionResult> Lwogin([FromBody] LoginRequest request, CancellationToken cancellationToken)
        {
            var result = await _mediator.Send(request, cancellationToken);

            return result.ToResponse();
        }
    }
}
