namespace WearCast.Api.Features.Sellers.GetSeller
{
    [Route("api/sellers")]
    [ApiController]
    [Authorize(Roles = $"{DefaultRoles.SellerManager},{DefaultRoles.SuperAdmin},{DefaultRoles.VendorAdmin}")]
    [Tags("Seller Profile")]
    public class GetSellerEndpoint(IMediator mediator) : ControllerBase
    {
        private readonly IMediator _mediator = mediator;

        [HttpGet("profile")]
        public async Task<IActionResult> GetProfile([FromQuery] GetSellerRequest request, CancellationToken cancellationToken)
        {
            var result = await _mediator.Send(request, cancellationToken);

            return result.ToResponse();
        }
    }
}