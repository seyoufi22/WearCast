namespace WearCast.Api.Features.DesignedProductManagement.CustomerDesigns.AddCustomerDesign
{
    [Route("api/customers/me/designs")]
    [ApiController]
    [Tags("Customer Design")]
    [Authorize(Roles = DefaultRoles.Customer)]
    public class AddCustomerDesignEndPoint(IMediator mediator) : ControllerBase
    {
        private readonly IMediator _mediator = mediator;
        [HttpPost]
        public async Task<IActionResult> Add([FromForm] AddCustomerDesignForm form, CancellationToken cancellationToken)
        {
            var request = new AddCustomerDesignRequest(
                    form.Name,
                    form.ViewDesignsJson,
                    form.FrontImage,
                    form.BackImage,
                    form.RightImage,
                    form.LeftImage,
                    form.AssetCount,
                    form.ProductId,
                    form.ProductColorId);

            var result = await _mediator.Send(request, cancellationToken);

            return result.ToResponse();
        }
        public record AddCustomerDesignForm(
            string ViewDesignsJson,
            int AssetCount,
            int ProductId,
            int ProductColorId,
            string? Name = null,
            IFormFile? FrontImage = null,
            IFormFile? BackImage = null,
            IFormFile? RightImage = null,
            IFormFile? LeftImage = null
        );
    }
}
