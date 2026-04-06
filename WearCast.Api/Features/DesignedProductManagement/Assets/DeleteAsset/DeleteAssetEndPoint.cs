namespace WearCast.Api.Features.DesignedProductManagement.Assets.DeleteAsset
{
    [Route("api/admin/design-assets")]
    [ApiController]
    [Authorize]
    [Tags("Assets")]
    public class DeleteAssetEndPoint(IMediator mediator) : ControllerBase
    {
        private readonly IMediator _mediator = mediator;

        [HttpDelete("{id:int}")]
        public async Task<IActionResult> Delete([FromRoute] int Id, CancellationToken cancellationToken)
        {
            var result = await _mediator.Send(new DeleteAssetRequest(Id), cancellationToken);

            return result.ToResponse();
        }
    }
}
