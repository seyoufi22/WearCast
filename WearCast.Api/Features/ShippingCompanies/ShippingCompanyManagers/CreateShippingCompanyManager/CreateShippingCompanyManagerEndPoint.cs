namespace WearCast.Api.Features.ShippingCompanies.ShippingCompanyManagers.CreateShippingCompanyManager
{
    [ApiController]
    [Route("api/shipping-company-managers")]
    [Authorize(Roles = $"{DefaultRoles.ShippingCompanyManager},{DefaultRoles.SuperAdmin}")]
    [Tags("Shipping Company Manager Profile")]
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
