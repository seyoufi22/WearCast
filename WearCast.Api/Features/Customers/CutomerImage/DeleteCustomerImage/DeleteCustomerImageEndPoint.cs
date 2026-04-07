namespace WearCast.Api.Features.Customers.CutomerImage.DeleteCustomerImage
{
    [Route("api/customers")]
    [ApiController]
    [Authorize(Roles = $"{DefaultRoles.Customer},{DefaultRoles.SuperAdmin}")]
    [Tags("Customer Profile")]
    [Consumes("multipart/form-data")]
    public class DeleteCustomerImageEndPoint(IMediator mediator) : ControllerBase
    {
        private readonly IMediator _mediator = mediator;
        [HttpDelete("profile-image")]
        public async Task<IActionResult> Delete([FromQuery] DeleteCustomerImageRequest request, CancellationToken cancellationToken)
        {
            var result = await _mediator.Send(request, cancellationToken);

            return result.ToResponse();
        }
    }
}
