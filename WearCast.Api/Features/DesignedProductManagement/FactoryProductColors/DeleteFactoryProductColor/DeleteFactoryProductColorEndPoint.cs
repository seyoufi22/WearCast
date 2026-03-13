namespace WearCast.Api.Features.DesignedProductManagement.FactoryProductColors.DeleteFactoryProductColor
{
    [Route("api/factory/products")]
    [ApiController]
    public class DeleteFactoryProductColorEndPoint(IMediator mediator) : ControllerBase
    {
        private readonly IMediator _mediator = mediator;

        [Authorize]
        [HttpDelete("{productId}/colors/{colorId}")]
        public async Task<IActionResult> Delete(
            [FromRoute] int productId,
            [FromRoute] int colorId,
            CancellationToken cancellationToken)
        {
            var result = await _mediator.Send(new DeleteFactoryProductColorRequest(colorId, productId), cancellationToken);

            return result.ToResponse();
        }
    }
}
