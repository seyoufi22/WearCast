namespace WearCast.Api.Features.ShippingCompanies.ShippingCompanyManagers.UpdateShippingCompanyManager
{
    [ApiController]
    [Route("api/shipping-company-managers")]
    [Authorize(Roles = $"{DefaultRoles.ShippingCompanyManager},{DefaultRoles.SuperAdmin}")]
    [Tags("Shipping Company Manager Profile")]
    public class UpdateShippingCompanyManagerEndPoint(IMediator mediator) : ControllerBase
    {
        private readonly IMediator _mediator = mediator;
        [HttpPut("profile")]
        public async Task<IActionResult> Update([FromBody] UpdateShippingCompanyManagerRequest request, CancellationToken cancellationToken)
        {
            var result = await _mediator.Send(request, cancellationToken);

            return result.ToResponse();
        }
    }
}
