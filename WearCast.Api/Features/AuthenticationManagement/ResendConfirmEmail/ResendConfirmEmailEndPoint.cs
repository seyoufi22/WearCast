
namespace WearCast.Api.Features.AuthenticationManagement.ResendConfirmEmail
{
    [Route("api/auth")]
    [ApiController]
    public class ResendConfirmEmailEndPoint(IMediator mediator) : ControllerBase
    {
        private readonly IMediator _mediator = mediator;

        [HttpPost("resend-confirmation-email")]
        public async Task<IActionResult> ResendConfirmationEmail([FromBody] ResendConfirmEmailRequest request, CancellationToken cancellationToken)
        {
            var result = await _mediator.Send(request, cancellationToken);

            return result.ToResponse();
        }
    }
}
