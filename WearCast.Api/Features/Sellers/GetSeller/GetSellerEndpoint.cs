namespace WearCast.Api.Features.Sellers.GetSeller;

[Route("api/sellers")]
[ApiController]
[Authorize(Roles = $"{DefaultRoles.SellerManager},{DefaultRoles.SuperAdmin}")]
[Tags("Seller Profile")]
public class GetSellerEndpoint(IMediator mediator) : ControllerBase
{
    private readonly IMediator _mediator = mediator;

    [HttpGet("{id:int?}")]
    public async Task<IActionResult> GetSeller(int? id, CancellationToken cancellationToken)
    {
        int targetId;

        if (User.IsInRole("SuperAdmin"))
        {
            if (!id.HasValue)
            {
                return BadRequest(new { message = "SuperAdmin must provide a Seller ID in the route." });
            }
            targetId = id.Value;
        }
        else
        {
            var sellerIdClaim = User.FindFirst("SellerId")?.Value;

            if (string.IsNullOrEmpty(sellerIdClaim) || !int.TryParse(sellerIdClaim, out targetId))
            {
                return Unauthorized(new { message = "SellerId is missing or invalid in the token." });
            }
        }

        var result = await _mediator.Send(new GetSellerRequest(targetId), cancellationToken);

        return result.ToResponse();
    }
}

