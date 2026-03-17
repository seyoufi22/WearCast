namespace WearCast.Api.Features.DesignedProductManagement.FactoryProductSizes.DeleteFactoryProductSize
{
    [Route("api/factory/product-sizes")]
    [ApiController]
    public class DeleteFactoryProductSizeEndPoint(IMediator mediator) : ControllerBase
    {
        private readonly IMediator _mediator = mediator;

        [HttpDelete("{Id:int}")]
        public async Task<IActionResult> Delete(
            [FromRoute] int Id,
            CancellationToken cancellationToken)
        {
            var result = await _mediator.Send(new DeleteFactoryProductSizeRequest(Id), cancellationToken);

            return result.ToResponse();
        }
    }
}
