

namespace WearCast.Api.Features.Sellers.SellerApplications.ApproveSellerApplication
{
    [Route("api/seller-applications")]
    [ApiController]
    public class ApproveSellerApplicationEndPoint(IMediator mediator) : ControllerBase
    {
        private readonly IMediator _mediator = mediator;
        [HttpPut("{email}/approve")]
        public async Task<IActionResult> Approve([FromRoute] string email, CancellationToken cancellationToken)
        {
            var request = new ApproveSellerApplicationRequest(email);

            var result = await _mediator.Send(request, cancellationToken);

            return result.ToResponse();
        }
    }
}
