namespace WearCast.Api.Features.Sellers.SellerManagers.GetSellerManager;

[Route("api/seller-managers")]
[ApiController]
[Authorize(Roles = $"{DefaultRoles.SellerManager},{DefaultRoles.SuperAdmin}")]
[Tags("Seller Manager Profile")]
public class GetSellerManagerEndPoint(IMediator mediator) : ControllerBase
{
    private readonly IMediator _mediator = mediator;

    [HttpGet("profile")]
    public async Task<IActionResult> GetProfile([FromQuery] GetSellerManagerRequest request, CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(request, cancellationToken);

        return result.ToResponse();
    }
}