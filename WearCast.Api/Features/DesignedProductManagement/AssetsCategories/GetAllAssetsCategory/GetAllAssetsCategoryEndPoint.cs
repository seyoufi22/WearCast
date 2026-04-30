namespace WearCast.Api.Features.DesignedProductManagement.AssetsCategories.GetAllAssetsCategory
{
    [Route("api/assets-categories")]
    [ApiController]
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
