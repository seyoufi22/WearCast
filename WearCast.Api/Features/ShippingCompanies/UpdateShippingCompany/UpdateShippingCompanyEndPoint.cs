namespace WearCast.Api.Features.ShippingCompanies.UpdateShippingCompany
{
    [Route("api/shipping-companies")]
    [ApiController]
    [Authorize(Roles = $"{DefaultRoles.ShippingCompanyManager},{DefaultRoles.SuperAdmin},{DefaultRoles.OperationsAdmin}")]
    [Tags("Shipping Company Profile")]
    public class UpdateShippingCompanyEndPoint(IMediator mediator) : ControllerBase
    {
        private readonly IMediator _mediator = mediator;

        [HttpPut("profile")]
        public async Task<IActionResult> UpdateProfile([FromBody] UpdateShippingCompanyRequest request, CancellationToken cancellationToken)
        {
            var result = await _mediator.Send(request, cancellationToken);

            return result.ToResponse();
        }
    }
}
