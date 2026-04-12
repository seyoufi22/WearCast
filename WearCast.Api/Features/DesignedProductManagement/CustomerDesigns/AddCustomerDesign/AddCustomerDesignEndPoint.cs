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
            IFormFile? FrontImage,
            IFormFile? BackImage,
            IFormFile? RightImage,
            IFormFile? LeftImage,
            int AssetCount,
            int ProductId,
            int ProductColorId
        );
    }
}
