namespace WearCast.Api.Features.DesignedProductManagement.FactoryProductColors.DeleteFactoryProductColor
{
    [Route("api/factory/products")]
    [ApiController]
    public class DeleteFactoryProductColorEndPoint(IMediator mediator) : ControllerBase
    {
        private readonly IMediator _mediator = mediator;

        [Authorize]
        [HttpDelete("{productSlug}/colors/{colorSlug}")]
        public async Task<IActionResult> Delete(
            [FromRoute] string productSlug,
            [FromRoute] string colorSlug,
            CancellationToken cancellationToken)
        {
            var result = await _mediator.Send(new DeleteFactoryProductColorRequest(productSlug, colorSlug), cancellationToken);

            return result.ToResponse();
        }
    }
}
