namespace WearCast.Api.Features.AuthenticationManagement.RefreshToken
{
    [Route("api/auth")]
    [ApiController]
    public class RefreshTokenEndPoint(IMediator mediator) : ControllerBase
    {
        private readonly IMediator _mediator = mediator;

        [HttpPost("refresh-token")]
        public async Task<IActionResult> Refresh([FromBody] RefreshTokenRequest request, CancellationToken cancellationToken)
        {
            var result = await _mediator.Send(request, cancellationToken);

            return result.ToResponse();
        }

    }
}
