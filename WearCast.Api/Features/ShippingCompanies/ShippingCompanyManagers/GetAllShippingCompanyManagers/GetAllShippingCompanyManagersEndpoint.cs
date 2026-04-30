namespace WearCast.Api.Features.ShippingCompanies.ShippingCompanyManagers.GetAllShippingCompanyManagers
{
    [ApiController]
    [Route("api/shipping-company-managers")]
    [Authorize(Roles = $"{DefaultRoles.ShippingCompanyManager},{DefaultRoles.SuperAdmin}")]
    [Tags("Shipping Company Manager Profile")]
    public class GetAllShippingCompanyManagersEndpoint(IMediator mediator) : ControllerBase
    {
        private readonly IMediator _mediator = mediator;

        [HttpGet("all")]
        public async Task<IActionResult> GetAllManagers(CancellationToken cancellationToken)
        {
            var result = await _mediator.Send(new GetAllShippingCompanyManagersRequest(), cancellationToken);

            return result.ToResponse();
        }
    }
}