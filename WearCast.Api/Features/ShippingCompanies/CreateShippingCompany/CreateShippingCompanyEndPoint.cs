namespace WearCast.Api.Features.ShippingCompanies.CreateShippingCompany
{
    [Route("api/shipping-companies")]
    public class CreateShippingCompanyEndPoint(IMediator mediator) : ControllerBase
    {
        private readonly IMediator _mediator = mediator;

        [HttpPost]
        public async Task<IActionResult> Create([FromForm] CreateShippingCompanyRequest request, CancellationToken cancellationToken)
        {
            var result = await _mediator.Send(request, cancellationToken);

            return result.ToResponse();
        }
    }
}
