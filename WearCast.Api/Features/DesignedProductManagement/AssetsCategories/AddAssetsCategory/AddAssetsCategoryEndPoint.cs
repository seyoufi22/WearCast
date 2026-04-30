namespace WearCast.Api.Features.DesignedProductManagement.AssetsCategories.AddAssetsCategory
{
    [Route("api/assets-categories")]
    [ApiController]
    [Authorize(Roles = $"{DefaultRoles.CatalogAdmin},{DefaultRoles.SuperAdmin}")]
    [Tags("AssetsCategory")]
    public class AddAssetsCategoryEndPoint(IMediator mediator) : ControllerBase
    {
        private readonly IMediator _mediator = mediator;
        [HttpPost]
        [Authorize]
        public async Task<IActionResult> Add([FromBody] AddAssetsCategoryRequest request, CancellationToken cancellationToken)
        {
            var result = await _mediator.Send(request, cancellationToken);

            return result.ToResponse();
        }
    }
}
