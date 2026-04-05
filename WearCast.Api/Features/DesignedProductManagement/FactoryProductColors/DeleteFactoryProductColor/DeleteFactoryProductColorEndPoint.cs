namespace WearCast.Api.Features.DesignedProductManagement.FactoryProductColors.DeleteFactoryProductColor
{
    [Route("api/factories/products")]
    [ApiController]
    [Tags("Factory Product Color")]
    public class DeleteFactoryProductColorEndPoint(IMediator mediator) : ControllerBase
    {
        private readonly IMediator _mediator = mediator;

        [Authorize]
        [HttpDelete("colors/{colorId}")]
        public async Task<IActionResult> Delete(
            [FromRoute] int colorId,
            CancellationToken cancellationToken)
        {
            var result = await _mediator.Send(new DeleteFactoryProductColorRequest(colorId), cancellationToken);

            return result.ToResponse();
        }
    }
}
