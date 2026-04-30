namespace WearCast.Api.Features.DesignedProductManagement.Assets.GetAsset
{
    [Route("api/design-assets")]
    [ApiController]
    [Tags("Assets")]
    public class GetAssetEndPoint(IMediator mediator) : ControllerBase
    {
        private readonly IMediator _mediator = mediator;

        [HttpGet("{id}")]
        public async Task<IActionResult> Get([FromRoute] int id, CancellationToken cancellationToken)
        {
            var result = await _mediator.Send(new GetAssetRequest(id), cancellationToken);

            return result.ToResponse();
        }
    }
}
