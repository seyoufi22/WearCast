namespace WearCast.Api.Features.DesignedProductManagement.Assets.UpdateAsset
{
    [Route("api/admin/design-assets")]
    [ApiController]
    [Authorize(Roles = $"{DefaultRoles.CatalogAdmin},{DefaultRoles.SuperAdmin}")]
    [Tags("Assets")]
    public class UpdateAssetEndPoint(IMediator mediator) : ControllerBase
    {
        private readonly IMediator _mediator = mediator;

        [HttpPut("{Id:int}")]
        [Consumes("multipart/form-data")]
        public async Task<IActionResult> Update([FromRoute] int Id, [FromForm] UpdateAssetForm form, CancellationToken cancellationToken)
        {
            var request = new UpdateAssetRequest(Id, form.Name, form.WidthPx, form.HeightPx, form.Image, form.CategoryId);

            var result = await _mediator.Send(request, cancellationToken);

            return result.ToResponse();
        }
        public record UpdateAssetForm(
        string Name,
        int WidthPx,
        int HeightPx,
        IFormFile? Image,
        int CategoryId
        );

    }
}
