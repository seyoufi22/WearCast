namespace WearCast.Api.Features.DesignedProductManagement.AssetsCategories.GetAllAssetsCategory
{
    [Route("api/assets-categories")]
    [ApiController]
    // [Authorize] // Uncomment this if only logged-in users/admins should see the categories
    [Tags("AssetsCategory")]
    public class GetAllAssetsCategoryEndPoint(IMediator mediator) : ControllerBase
    {
        private readonly IMediator _mediator = mediator;

        [HttpGet]
        public async Task<IActionResult> GetAll(CancellationToken cancellationToken)
        {
            var result = await _mediator.Send(new GetAllAssetsCategoryRequest(), cancellationToken);
            return result.ToResponse();
        }
    }
}
