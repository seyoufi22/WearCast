

namespace WearCast.Api.Features.DesignedProductManagement.Assets.GetAssetsByCategory
{
    [Route("api/design-assets")]
    [ApiController]
    // [Authorize] // يمكنك إزالة هذا السطر بالكامل إذا كانت الإضافة متاحة للزوار بدون تسجيل دخول
    [Tags("Assets")]
    public class GetAllAssetsEndPoint(IMediator mediator) : ControllerBase
    {
        private readonly IMediator _mediator = mediator;

        [HttpGet("category/{categoryId}")]
        public async Task<IActionResult> GetAll(
            [FromRoute] int categoryId,
            [FromQuery] int pageIndex = 1,
            [FromQuery] int pageSize = 10,
            CancellationToken cancellationToken = default)
        {
            var request = new GetAssetsByCategoryRequest(categoryId, pageIndex, pageSize);

            var result = await _mediator.Send(request, cancellationToken);

            return result.ToResponse();
        }
    }
}