namespace WearCast.Api.Features.Customers.CutomerImage.DeleteCustomerImage
{
    [Route("api/customer/profile-image")]
    [ApiController]
    [Authorize(Roles = $"{DefaultRoles.Customer},{DefaultRoles.SuperAdmin}")]
    public class DeleteCustomerImageEndPoint(IMediator mediator) : ControllerBase
    {
        private readonly IMediator _mediator = mediator;
        [HttpDelete]
        [Tags("Customer Profile")]
        [Consumes("multipart/form-data")]
        public async Task<IActionResult> Delete([FromBody] DeleteCustomerImageRequest request, CancellationToken cancellationToken)
        {
            var result = await _mediator.Send(request, cancellationToken);

            return result.ToResponse();
        }
    }
}
