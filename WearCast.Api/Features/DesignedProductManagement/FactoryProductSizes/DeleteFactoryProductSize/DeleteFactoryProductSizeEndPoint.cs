namespace WearCast.Api.Features.DesignedProductManagement.FactoryProductSizes.DeleteFactoryProductSize
{
    [Route("api/factories/product-sizes")]
    [ApiController]
    [Authorize(Roles = $"{DefaultRoles.FactoryManager},{DefaultRoles.CatalogAdmin},{DefaultRoles.SuperAdmin}")]
    [Tags("Factory Product size")]
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
