namespace WearCast.Api.Features.AuthenticationManagement.SellerApplications.ConfirmSellerEmail
{

    [Route("api/seller-applications")]
    [ApiController]
    public class ConfirmSellerEmailEndPoint(IMediator mediator) : ControllerBase
    {
        private readonly IMediator _mediator = mediator;
        [HttpPost("{email}/confirm-email")]
        public async Task<IActionResult> ConfirmEmail([FromRoute] string email, [FromBody] ConfirmSellerEmailBody body, CancellationToken cancellationToken)
        {
            var request = new ConfirmSellerEmailRequest(email, body.Code);
            var result = await _mediator.Send(request, cancellationToken);

            return result.ToResponse();
        }
        public record ConfirmSellerEmailBody(string Code);
    }

}
