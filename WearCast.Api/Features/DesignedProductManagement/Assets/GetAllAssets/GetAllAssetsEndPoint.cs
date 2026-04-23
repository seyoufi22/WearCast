namespace WearCast.Api.Features.DesignedProductManagement.Assets.GetAllAssets
{
    [Route("api/design-assets")]
    [ApiController]
    [Tags("Design Assets")]
    public class GetAllDesignAssetsEndPoint(IMediator mediator) : ControllerBase
    {
        [HttpGet]
        public async Task<IActionResult> GetAll(
            [FromQuery] int pageIndex = 1,
            [FromQuery] int pageSize = 10,
            [FromQuery] int? categoryId = null,
            [FromQuery] string? searchTerm = null,
            CancellationToken cancellationToken = default)
        {
            var request = new GetAllAssetsRequest(pageIndex, pageSize, categoryId, searchTerm);

            var result = await mediator.Send(request, cancellationToken);

            return result.ToResponse();
        }
    }
}
