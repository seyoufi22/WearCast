using System.Security.Claims;

namespace WearCast.Api.Features.Sellers.SellerManagers.DeleteSellerManager;

[Route("api/sellers/managers")]
[ApiController]
[Authorize(Roles = $"{DefaultRoles.SellerManager},{DefaultRoles.SuperAdmin},{DefaultRoles.VendorAdmin}")]
[Tags("Seller Managers")]
public class DeleteSellerManagerEndpoint(IMediator mediator) : ControllerBase
{
    private readonly IMediator _mediator = mediator;

    [HttpDelete("{sellerManagerId:int}")]
    public async Task<IActionResult> DeleteSellerManager(
        [FromRoute] int sellerManagerId,
        [FromBody] DeleteSellerManagerBody body,
        CancellationToken cancellationToken)
    {
        var currentUserId = User.FindFirstValue(ClaimTypes.NameIdentifier);

        if (string.IsNullOrEmpty(currentUserId))
            return Unauthorized();


        bool isAdmin = User.IsInRole(DefaultRoles.SuperAdmin) || User.IsInRole(DefaultRoles.VendorAdmin);

        var request = new DeleteSellerManagerRequest(sellerManagerId, currentUserId, isAdmin, body.Reason);
        var result = await _mediator.Send(request, cancellationToken);

        return result.ToResponse();
    }
}