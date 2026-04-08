using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Drawing;
using System.Security.Claims;
using WearCast.Api.Features.FixedProductColor.DeleteFixedProductColor.DTOs;
using WearCast.Api.Features.FixedProductColor.DeleteFixedProductImage.DTOs;

namespace WearCast.Api.Features.FixedProductColor.DeleteFixedProductImage;

[Tags("FixedProductColor")]
[Route("api/FixedProductColor")]
[ApiController]
public class DeleteFixedProductImageEndpoint(ISender sender) : ControllerBase
{

    [Authorize(Roles = "SellerManager,SuperAdmin")]
    [HttpDelete("DeleteImage/{ImageId:int}", Name = "DeleteImageFromProductColor")]
    public async Task<IActionResult> DeleteImage(int ImageId, CancellationToken cancellationToken)
    {
        Result result;
        var Role = User.FindFirstValue(ClaimTypes.Role);
        if (Role == "SuperAdmin")
            result = await sender.Send(new DeleteFixedProductImageRequestDto(ImageId, 0, true));
        else
        {
            var sellerId = User.FindFirstValue("SellerId");

            if (string.IsNullOrEmpty(sellerId))
                return Unauthorized();

            result = await sender.Send(new DeleteFixedProductImageRequestDto(ImageId, int.Parse(sellerId), false));
        }

        if (result.IsFailure)
        {
            return StatusCode(result.Error.StatusCode ?? StatusCodes.Status400BadRequest, result.Error);
        }
        return Ok(new { Message = "Image deleted successfully." });
    }
}