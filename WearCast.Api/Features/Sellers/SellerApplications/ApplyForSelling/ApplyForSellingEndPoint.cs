namespace WearCast.Api.Features.Sellers.SellerApplications.ApplyForSelling
{
    [Route("api/seller-applications")]
    [ApiController]
    public class ApplyForSellingEndPoint(IMediator mediator) : ControllerBase
    {
        private readonly IMediator _mediator = mediator;

        [HttpPost]
        public async Task<IActionResult> Apply([FromForm] ApplyForSellingRequest request, CancellationToken cancellationToken)
        {
            var result = await _mediator.Send(request, cancellationToken);

            return result.ToResponse();
        }
    }
}
