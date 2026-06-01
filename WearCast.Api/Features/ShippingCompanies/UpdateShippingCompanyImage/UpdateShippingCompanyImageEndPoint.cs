namespace WearCast.Api.Features.ShippingCompanies.UpdateShippingCompanyImage
{
    [Route("api/shipping-companies/profile-image")]
    [ApiController]
    [Authorize(Roles = $"{DefaultRoles.ShippingCompanyManager},{DefaultRoles.SuperAdmin},{DefaultRoles.OperationsAdmin}")]
    [Tags("Shipping Company Profile")]
    [Consumes("multipart/form-data")]
    public class UpdateShippingCompanyImageEndPoint(IMediator mediator) : ControllerBase
    {
        private readonly IMediator _mediator = mediator;

        [HttpPut]
        public async Task<IActionResult> Update([FromForm] UpdateShippingCompanyImageRequest request, CancellationToken cancellationToken)
        {
            var result = await _mediator.Send(request, cancellationToken);

            return result.ToResponse();
        }
    }
}
