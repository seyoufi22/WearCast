namespace WearCast.Api.Features.Customers.DeleteCustomer
{
    [Route("api/customers")]
    [ApiController]
    [Authorize(Roles = DefaultRoles.SuperAdmin)]
    [Tags("Customer Profile")]
    public class DeleteCustomerEndpoint(IMediator mediator) : ControllerBase
    {
        private readonly IMediator _mediator = mediator;

        [HttpDelete("{id:int}")]
        public async Task<IActionResult> DeleteCustomer(
            [FromRoute] int id,
            [FromBody] DeleteCustomerBody body,
            CancellationToken cancellationToken)
        {
            var request = new DeleteCustomerRequest(id, body.Reason);
            var result = await _mediator.Send(request, cancellationToken);

            return result.ToResponse();
        }
    }
}