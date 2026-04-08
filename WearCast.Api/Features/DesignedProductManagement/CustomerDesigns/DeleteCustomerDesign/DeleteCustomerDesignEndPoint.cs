namespace WearCast.Api.Features.DesignedProductManagement.CustomerDesigns.DeleteCustomerDesign
{
    [Route("api/customers/me/designs")]
    [ApiController]
    [Authorize(Roles = DefaultRoles.Customer)]
    [Tags("Customer Design")]
    public class DeleteCustomerDesignEndPoint(IMediator mediator) : ControllerBase
    {
        private readonly IMediator _mediator = mediator;

        [HttpDelete("{Id:int}")]
        public async Task<IActionResult> Delete([FromRoute] int Id, CancellationToken cancellationToken)
        {
            var result = await _mediator.Send(new DeleteCustomerDesignRequest(Id), cancellationToken);

            return result.ToResponse();
        }
    }
}
