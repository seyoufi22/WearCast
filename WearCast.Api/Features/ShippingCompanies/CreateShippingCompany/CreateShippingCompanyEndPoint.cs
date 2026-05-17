namespace WearCast.Api.Features.ShippingCompanies.CreateShippingCompany
{
    [ApiController]
    [Route("api/shipping-companies")]
    [Tags("Shipping Company Profile")]
    [Authorize(Roles = $"{DefaultRoles.SuperAdmin},{DefaultRoles.OperationsAdmin}")]
    public class CreateShippingCompanyEndPoint(IMediator mediator) : ControllerBase
    {
        private readonly IMediator _mediator = mediator;

        [HttpPost]
        [Consumes("multipart/form-data")]
        public async Task<IActionResult> Create([FromForm] CreateShippingCompanyRequest request, CancellationToken cancellationToken)
        {
            var result = await _mediator.Send(request, cancellationToken);

            return result.ToResponse();
        }
    }
}
