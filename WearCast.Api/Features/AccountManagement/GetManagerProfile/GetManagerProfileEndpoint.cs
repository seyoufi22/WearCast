namespace WearCast.Api.Features.Sellers.GetSellerProfile;

using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using WearCast.Api.Features.AccountManagement.GetManagerProfile;

[Route("api/sellers")]
[ApiController]
[Authorize]
[Tags("manager Profile")]
public class GetSellerProfileEndPoint(IMediator mediator) : ControllerBase
{
    [HttpGet("profile")]
    public async Task<IActionResult> GetProfile(CancellationToken cancellationToken)
    {

        string? id = User.FindFirstValue(ClaimTypes.NameIdentifier) ?? User.FindFirst("sub")?.Value;

        if (string.IsNullOrEmpty(id))
        {
            return Unauthorized(new { message = "User ID is missing from the token." });
        }

        var result = await mediator.Send(new GetManagerProfileRequest(id), cancellationToken);

        return result.ToResponse();
    }
}