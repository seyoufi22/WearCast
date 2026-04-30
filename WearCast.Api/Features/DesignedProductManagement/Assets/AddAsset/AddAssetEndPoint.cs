namespace WearCast.Api.Features.DesignedProductManagement.Assets.AddAsset
{
    [Route("api/admin/design-assets")]
    [ApiController]
    [Authorize(Roles = $"{DefaultRoles.CatalogAdmin},{DefaultRoles.SuperAdmin}")]
    [Tags("Assets")]
    public class AddAssetEndPoint(IMediator mediator) : ControllerBase
    {
        private readonly IMediator _mediator = mediator;
        [HttpPost]
        [Consumes("multipart/form-data")]
        public async Task<IActionResult> Add([FromForm] AddAssetRequest request, CancellationToken cancellationToken)
        {
            var result = await _mediator.Send(request, cancellationToken);

            return result.ToResponse();
        }
    }
}
