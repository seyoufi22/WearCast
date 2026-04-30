namespace WearCast.Api.Features.DesignedProductManagement.AssetsCategories.DeleteAssetsCategory
{
    [ApiController]
    [Route("api/assets-categories")]
    [Authorize(Roles = $"{DefaultRoles.CatalogAdmin},{DefaultRoles.SuperAdmin}")]
    [Tags("AssetsCategory")]
    public class DeleteAssetsCategoryEndPoint(IMediator mediator) : ControllerBase
    {
        private readonly IMediator _mediator = mediator;

        [HttpDelete("{id:int}")]
        public async Task<IActionResult> Delete(int id, CancellationToken cancellationToken)
        {
            var result = await _mediator.Send(new DeleteAssetsCategoryRequest(id), cancellationToken);

            return result.ToResponse();
        }
    }
}
