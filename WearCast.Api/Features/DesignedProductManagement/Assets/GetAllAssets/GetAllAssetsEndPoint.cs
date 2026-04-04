namespace WearCast.Api.Features.DesignedProductManagement.Assets.GetAllAssets
{
    [Route("api/design-assets")]
    [ApiController]
    // [Authorize] // يمكنك إزالة هذا السطر بالكامل إذا كانت الإضافة متاحة للزوار بدون تسجيل دخول
    public class GetAllAssetsEndPoint(IMediator mediator) : ControllerBase
    {
        private readonly IMediator _mediator = mediator;

        [HttpGet("category/{categoryId}")]
        public async Task<IActionResult> GetAll([FromRoute] int categoryId, CancellationToken cancellationToken)
        {
            var result = await _mediator.Send(new GetAllAssetsRequest(categoryId), cancellationToken);

            return result.ToResponse();
        }
    }
}
