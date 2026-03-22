namespace WearCast.Api.Features.DesignedProductManagement.FactoryProducts.DeleteDesignedProduct
{
    [Route("api/factory/products")]
    [ApiController]
    public class DeleteDesignedProductEndPoint(IMediator mediator) : ControllerBase
    {
        private readonly IMediator _mediator = mediator;
        [Authorize]
        [HttpDelete("{Id}")]
        public async Task<IActionResult> Delete([FromRoute] int Id, CancellationToken cancellationToken)
        {
            var result = await _mediator.Send(new DeleteDesignedProductRequest(Id), cancellationToken);

            return result.ToResponse();
        }
    }
}
