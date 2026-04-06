namespace WearCast.Api.Features.AuthenticationManagement.ForgetPassword
{
    [Route("api/auth")]
    [ApiController]
    [Tags("Auth")]
    public class ForgetPasswordEndPoint(IMediator mediator) : ControllerBase
    {
        private readonly IMediator _mediator = mediator;

        [HttpPost("forget-password")]
        public async Task<IActionResult> ForgetPassword([FromBody] ForgetPasswordRequest request, CancellationToken cancellationToken)
        {
            var result = await _mediator.Send(request, cancellationToken);

            return result.ToResponse();
        }
    }
}
