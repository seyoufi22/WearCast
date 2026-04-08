

namespace WearCast.Api.Features.Sellers.SellerApplications.ResendSellerManagerConfirmEmail
{
    [Route("api/seller-applications")]
    [ApiController]
    public class ResendConfirmSellerEmailEndPoint(IMediator mediator) : ControllerBase
    {
        private readonly IMediator _mediator = mediator;
        [HttpPost("{email}/resend-confirm-email")]
        public async Task<IActionResult> ResendSellerConfirmEmail([FromRoute] string email, CancellationToken cancellationToken)
        {
            var request = new ResendConfirmSellerManagerEmailRequest(email);
            var result = await _mediator.Send(request, cancellationToken);

            return result.ToResponse();
        }
    }
}
