namespace WearCast.Api.Features.ShippingCompanies.DeleteShippingCompany
{
    [Route("api/shipping-companies")]
    [ApiController]
    [Authorize(Roles = $"{DefaultRoles.SuperAdmin},{DefaultRoles.OperationsAdmin}")]
    [Tags("Shipping Company Profile")]
    public class DeleteShippingCompanyEndpoint(IMediator mediator) : ControllerBase
    {
        private readonly IMediator _mediator = mediator;

        [HttpDelete("{id:int}")]
        public async Task<IActionResult> DeleteShippingCompany(
            [FromRoute] int id,
            [FromBody] DeleteShippingCompanyBody body,
            CancellationToken cancellationToken)
        {
            var request = new DeleteShippingCompanyRequest(id, body.Reason);
            var result = await _mediator.Send(request, cancellationToken);

            return result.ToResponse();
        }
    }
}