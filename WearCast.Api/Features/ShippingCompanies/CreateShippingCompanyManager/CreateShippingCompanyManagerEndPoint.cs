namespace WearCast.Api.Features.ShippingCompanies.CreateShippingCompanyManager
{
    [ApiController]
    [Route("api/shipping-company-managers")]
    public class CreateShippingCompanyManagerEndPoint(IMediator mediator) : ControllerBase
    {
        private readonly IMediator _mediator = mediator;

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateShippingCompanyManagerRequest request, CancellationToken cancellationToken)
        {
            var result = await _mediator.Send(request, cancellationToken);

            return result.ToResponse();
        }
    }
}
