namespace WearCast.Api.Features.DesignedProductManagement.AssetsCategories.UpdateAssetsCategory
{
    [ApiController]
    [Route("api/assets-categories")]
    [Tags("AssetsCategory")]
    [Authorize(Roles = $"{DefaultRoles.CatalogAdmin},{DefaultRoles.SuperAdmin}")]
    public class UpdateAssetsCategoryEndPoint(IMediator mediator) : ControllerBase
    {
        private readonly IMediator _mediator = mediator;

        [HttpPut("{id}")]
        [Authorize]
        public async Task<IActionResult> Update(int id, [FromBody] UpdateAssetsCategoryBody body, CancellationToken cancellationToken)
        {
            var result = await _mediator.Send(new UpdateAssetsCategoryRequest(id, body.Name), cancellationToken);

            return result.ToResponse();
        }

    }
    public record UpdateAssetsCategoryBody(string Name);
}
