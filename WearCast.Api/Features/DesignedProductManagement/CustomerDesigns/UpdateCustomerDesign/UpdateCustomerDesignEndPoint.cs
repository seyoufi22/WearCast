namespace WearCast.Api.Features.DesignedProductManagement.CustomerDesigns.UpdateCustomerDesign
{
    [Route("api/customers/me/designs")]
    [ApiController]
    [Authorize(Roles = DefaultRoles.Customer)]
    [Tags("Customer Design")]
    public class UpdateCustomerDesignEndPoint(IMediator mediator) : ControllerBase
    {
        private readonly IMediator _mediator = mediator;

        [HttpPut("{Id:int}")]
        public async Task<IActionResult> Update([FromRoute] int Id, [FromBody] UpdateCustomerDesignBody body, CancellationToken cancellationToken)
        {
            var result = await _mediator.Send(new UpdateCustomerDesignRequest(Id, body.ViewDesignsJson, body.NewProductColorId));

            return result.ToResponse();
        }
        public record UpdateCustomerDesignBody(string ViewDesignsJson, int NewProductColorId);
    }
}
