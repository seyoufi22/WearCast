namespace WearCast.Api.Features.AuthenticationManagement.ConfirmEmail
{
    [Route("auth")]
    [ApiController]
    public class ConfirmEmailEndPoint(IMediator mediator) : ControllerBase
    {
        private readonly IMediator _mediator = mediator;
        [HttpPost("confirm-email")]
        public async Task<IActionResult> ConfirmEmail([FromBody] ConfirmEmailRequest request, CancellationToken cancellationToken)
        {
            var result = await _mediator.Send(request, cancellationToken);

            return result.IsSuccess ? Ok() : result.ToProblem();
        }
    }
}
