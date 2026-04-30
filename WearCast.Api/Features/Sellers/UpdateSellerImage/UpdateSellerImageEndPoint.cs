namespace WearCast.Api.Features.Sellers.UpdateSellerImage
{
    [Route("api/sellers/profile-image")]
    [ApiController]
    [Authorize(Roles = $"{DefaultRoles.SellerManager},{DefaultRoles.VendorAdmin},{DefaultRoles.SuperAdmin}")]
    [Tags("Seller Profile")]
    [Consumes("multipart/form-data")]
    public class UpdateSellerImageEndPoint(IMediator mediator) : ControllerBase
    {
        private readonly IMediator _mediator = mediator;

        [HttpPut]
        public async Task<IActionResult> Update([FromForm] UpdateSellerImageRequest request, CancellationToken cancellationToken)
        {
            var result = await _mediator.Send(request, cancellationToken);

            return result.ToResponse();
        }
    }
}
