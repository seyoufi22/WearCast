namespace WearCast.Api.Features.DesignedProductManagement.CustomerDesigns.UpdateCustomerDesign
{
    [Route("api/customers/me/designs")]
    [ApiController]
    [Authorize(Roles = DefaultRoles.Customer)]
    [Tags("Customer Design")]
    public class UpdateCustomerDesignEndPoint(IMediator mediator) : ControllerBase
    {
        private readonly IMediator _mediator = mediator;

        [HttpPut("{id:int}")]
        public async Task<IActionResult> Update(
            [FromRoute] int id,
            [FromForm] UpdateCustomerDesignForm form,
            CancellationToken cancellationToken)
        {
            var result = await _mediator.Send(
                new UpdateCustomerDesignRequest(
                    id,
                    form.ViewDesignsJson,
                    form.FrontImage,
                    form.BackImage,
                    form.RightImage,
                    form.LeftImage,
                    form.AssetCount),
                cancellationToken);

            return result.ToResponse();
        }

        public record UpdateCustomerDesignForm(
            string ViewDesignsJson,
            IFormFile? FrontImage,
            IFormFile? BackImage,
            IFormFile? RightImage,
            IFormFile? LeftImage,
            int AssetCount
        );
    }
}