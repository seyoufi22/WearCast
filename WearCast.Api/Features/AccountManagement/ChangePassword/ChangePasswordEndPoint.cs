namespace WearCast.Api.Features.AccountManagement.ChangePassword
{
    [Route("me")]
    [ApiController]
    [Authorize]
    public class ChangePasswordEndPoint(IMediator mediator) : ControllerBase
    {
        private readonly IMediator _mediator = mediator;

        [HttpPut("change-password")]
        public async Task<IActionResult> ChangePassword([FromBody]ChangePasswordRequest request, CancellationToken cancellationToken)
        {
            var result = await _mediator.Send(request, cancellationToken);

            return result.ToResponse();
        }
    }
}
