using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using WearCast.Api.Features.FixedProductColor.AdjustStockFixedProductSize.DTOs;
using WearCast.Api.Features.FixedProductColor.UpdateFixedProductColor.DTOs;

namespace WearCast.Api.Features.FixedProductColor.UpdateFixedProductColor;

[Tags("FixedProductColor")]
[Route("api/FixedProductColor")]
[ApiController]
public class UpdateFixedProductColorEndpoint(ISender sender) : ControllerBase
{
    [Authorize(Roles = $"{DefaultRoles.SellerManager},{DefaultRoles.SuperAdmin},{DefaultRoles.CatalogAdmin}")]
    [HttpPut("UpdateColor")]
    [Consumes("multipart/form-data")]
    public async Task<IActionResult> UpdateColor([FromForm] UpdateFixedProductColorRequestDto request, CancellationToken cancellationToken)
    {

        Result result;
        var Role = User.FindFirstValue(ClaimTypes.Role);
        if (Role == DefaultRoles.SuperAdmin || Role == DefaultRoles.CatalogAdmin)
            result = await sender.Send(new UpdateFixedProductColorCommandDto(request, 0, true));
        else
        {
            var sellerId = User.FindFirstValue("SellerId");

            if (string.IsNullOrEmpty(sellerId))
                return Unauthorized();

            result = await sender.Send(new UpdateFixedProductColorCommandDto(request, int.Parse(sellerId), false));
        }

        if (result.IsFailure)
        {
            return StatusCode(result.Error.StatusCode ?? StatusCodes.Status400BadRequest, result.Error);
        }
        return Ok(new { Message = "Product color updated successfully." });
    }
}