namespace WearCast.Api.Features.DesignedProductManagement.CustomerDesigns.DeleteCustomerDesignImage
{
    [Route("api/customers/me/designs")]
    [ApiController]
    [Authorize(Roles = DefaultRoles.Customer)]
    [Tags("Customer Design")]
    public class DeleteCustomerDesignImageEndPoint(IMediator mediator) : ControllerBase
    {
        private readonly IMediator _mediator = mediator;

        [HttpDelete("{designId:int}/images/{side}")]
        public async Task<IActionResult> DeleteImage(
            [FromRoute] int designId,
            [FromRoute] ViewSide side,
            CancellationToken cancellationToken)
        {
            var result = await _mediator.Send(
                new DeleteCustomerDesignImageRequest(designId, side),
                cancellationToken);

            return result.ToResponse();
        }
    }
}
