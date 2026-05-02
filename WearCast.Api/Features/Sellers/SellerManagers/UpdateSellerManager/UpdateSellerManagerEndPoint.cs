namespace WearCast.Api.Features.Sellers.SellerManagers.UpdateSellerManager
{
    [Route("api/seller-managers")]
    [ApiController]
    [Authorize(Roles = $"{DefaultRoles.SellerManager},{DefaultRoles.VendorAdmin},{DefaultRoles.SuperAdmin}")]
    [Tags("Seller Manager Profile")]
    public class UpdateSellerManagerEndPoint(IMediator mediator) : ControllerBase
    {
        private readonly IMediator _mediator = mediator;
        [HttpPut("profile")]
        public async Task<IActionResult> UpdateProfile([FromBody] UpdateSellerManagerRequest request, CancellationToken cancellationToken)
        {
            var result = await _mediator.Send(request, cancellationToken);

            return result.ToResponse();
        }
    }
}
