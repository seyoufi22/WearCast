namespace WearCast.Api.Features.Customers.CutomerImage.UpdateCusomerImage
{
    [Route("api/customers")]
    [ApiController]
    [Authorize(Roles = $"{DefaultRoles.Customer},{DefaultRoles.SuperAdmin}")]
    [Tags("Customer Profile")]
    [Consumes("multipart/form-data")]
    public class UpdateCustomerImageEndPoint(IMediator mediator) : ControllerBase
    {
        private readonly IMediator _mediator = mediator;
        [HttpPut("profile-image")]
        public async Task<IActionResult> Update([FromForm] UpdateCustomerImageRequest request, CancellationToken cancellationToken)
        {
            var result = await _mediator.Send(request, cancellationToken);

            return result.ToResponse();
        }
    }
}
