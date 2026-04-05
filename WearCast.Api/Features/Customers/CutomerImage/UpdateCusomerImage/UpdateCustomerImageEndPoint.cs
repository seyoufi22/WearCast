namespace WearCast.Api.Features.Customers.CutomerImage.UpdateCusomerImage
{
    [Route("api/customer/profile-image")]
    [ApiController]
    [Authorize(Roles = $"{DefaultRoles.Customer},{DefaultRoles.SuperAdmin}")]
    public class UpdateCustomerImageEndPoint(IMediator mediator) : ControllerBase
    {
        private readonly IMediator _mediator = mediator;
        [HttpPut]
        [Tags("Customer Profile")]
        [Consumes("multipart/form-data")]
        public async Task<IActionResult> Update([FromForm] UpdateCustomerImageRequest request, CancellationToken cancellationToken)
        {
            var result = await _mediator.Send(request, cancellationToken);

            return result.ToResponse();
        }
    }
}
