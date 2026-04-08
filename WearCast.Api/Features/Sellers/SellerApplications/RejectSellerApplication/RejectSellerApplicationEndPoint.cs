namespace WearCast.Api.Features.Sellers.SellerApplications.RejectSellerApplication
{
    [Route("api/seller-applications")]
    [ApiController]
    [Tags("Seller Applications")]
    public class RejectSellerApplicationEndPoint(IMediator mediator) : ControllerBase
    {
        private readonly IMediator _mediator = mediator;

        [HttpPut("{email}/reject")]
        public async Task<IActionResult> Reject([FromRoute] string email, [FromBody] RejectApplicationBody body, CancellationToken cancellationToken)
        {
            var request = new RejectSellerApplicationRequest(email, body.Reason);

            var result = await _mediator.Send(request, cancellationToken);

            return result.ToResponse();
        }
    }
    public record RejectApplicationBody(string Reason);
}
