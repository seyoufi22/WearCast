

namespace WearCast.Api.Features.Sellers.SellerApplications.ApproveSellerApplication
{
    [Route("api/seller-applications")]
    [ApiController]
    [Tags("Seller Applications")]
    [Authorize(Roles = $"{DefaultRoles.VendorAdmin},{DefaultRoles.SuperAdmin}")]
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
