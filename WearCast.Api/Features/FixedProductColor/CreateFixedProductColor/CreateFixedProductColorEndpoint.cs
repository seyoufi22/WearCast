using System.Security.Claims;
using WearCast.Api.Features.FixedProductColor.CreateFixedProductColor.DTOs;

namespace WearCast.Api.Features.FixedProductColor.CreateFixedProductColor;

[Tags("FixedProductColor")]
[Route("api/FixedProductColor")]
[ApiController]
public class CreateFixedProductColorEndpoint(ISender sender) : ControllerBase
{
    private readonly ISender _sender = sender;
    [Authorize(Roles = $"{DefaultRoles.SellerManager}")]
    [HttpPost("CreateProductColor")]
    [Consumes("multipart/form-data")]
    public async Task<ActionResult<int>> Create(
        [FromForm] CreateFixedProductColorRequestDto request,
        CancellationToken cancellationToken)
    {
        var sellerId = User.FindFirstValue("SellerId");

        if (string.IsNullOrEmpty(sellerId))
            return Unauthorized();

        var result = await _sender.Send(new CreateFixedProductColorCommandDto(request,int.Parse(sellerId)), cancellationToken);
        if (result.IsFailure)
        {
            return StatusCode(result.Error.StatusCode ?? StatusCodes.Status400BadRequest, result.Error);
        }
        return Ok(new { Message = "color added successfully." });
    }
}