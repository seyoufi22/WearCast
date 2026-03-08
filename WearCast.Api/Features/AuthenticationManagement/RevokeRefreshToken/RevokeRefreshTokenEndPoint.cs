namespace WearCast.Api.Features.AuthenticationManagement.RevokeRefreshToken
{
    [Route("api/auth")]
    [ApiController]
    public class RevokeRefreshTokenEndPoint(IMediator mediator) : ControllerBase
    {
        private readonly IMediator _mediator = mediator;

        [HttpPost("revoke-refresh-token")]
        public async Task<IActionResult> Revoke([FromBody] RevokeRefreshTokenRequest request, CancellationToken cancellationToken)
        {
            var result = await _mediator.Send(request, cancellationToken);

            return result.ToResponse();
        }
    }
}
