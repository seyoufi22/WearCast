namespace WearCast.Api.Features.Sellers.SellerManagers.CreateSellerManager
{
    [Route("api/seller-managers")]
    [ApiController]
    public class CreateSellerManagerEndPoint(IMediator mediator) : ControllerBase
    {
        private readonly IMediator _mediator = mediator;

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateSellerManagerRequest request, CancellationToken cancellationToken)
        {
            var result = await _mediator.Send(request, cancellationToken);

            return result.ToResponse();
        }
    }
}
