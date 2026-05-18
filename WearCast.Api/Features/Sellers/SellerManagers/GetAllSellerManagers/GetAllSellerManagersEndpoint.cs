namespace WearCast.Api.Features.Sellers.SellerManagers.GetAllSellerManagers
{
    [Route("api/seller-managers")]
    [ApiController]
    [Authorize(Roles = $"{DefaultRoles.SellerManager},{DefaultRoles.SuperAdmin},{DefaultRoles.VendorAdmin}")]
    [Tags("Seller Manager Profile")]
    public class GetAllSellerManagersEndpoint(IMediator mediator) : ControllerBase
    {
        private readonly IMediator _mediator = mediator;

        [HttpGet("all")]
        public async Task<IActionResult> GetAllManagers([FromQuery] GetAllSellerManagersRequest request, CancellationToken cancellationToken)
        {
            var result = await _mediator.Send(request, cancellationToken);

            return result.ToResponse();
        }
    }
}