using WearCast.Api.Features.Customers.DeleteCustomer;

namespace WearCast.Api.Features.Sellers.DeleteSeller
{
    [Route("api/sellers")]
    [ApiController]
    [Authorize(Roles = DefaultRoles.SuperAdmin)]  
    [Tags("Seller Profile")]
    public class DeleteSellerEndpoint(IMediator mediator) : ControllerBase
    {
        private readonly IMediator _mediator = mediator;

        [HttpDelete("{id:int}")]  
        public async Task<IActionResult> DeleteSeller(
                        [FromRoute] int id,
                        [FromBody] DeleteSellerBody body,
            CancellationToken cancellationToken)
        {
            var request = new DeleteSellerRequest(id, body.Reason);
            var result = await _mediator.Send(request, cancellationToken);

            return result.ToResponse();
        }
    }
}