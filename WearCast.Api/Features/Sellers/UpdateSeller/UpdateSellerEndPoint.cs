namespace WearCast.Api.Features.Sellers.UpdateSeller
{
    [Route("api/sellers")]
    [ApiController]
    [Authorize(Roles = $"{DefaultRoles.SellerManager},{DefaultRoles.SuperAdmin}")]
    [Tags("Seller Profile")]
    public class UpdateSellerEndPoint(IMediator mediator) : ControllerBase
    {
        private readonly IMediator _mediator = mediator;

        [HttpPut("profile")]
        public async Task<IActionResult> UpdateProfile([FromBody] UpdateSellerRequest request, CancellationToken cancellationToken)
        {
            var result = await _mediator.Send(request, cancellationToken);

            return result.ToResponse();
        }
    }
}
