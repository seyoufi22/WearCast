using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WearCast.Api.Features.FixedProductColor.DeleteFixedProductImage.DTOs;

namespace WearCast.Api.Features.FixedProductColor.DeleteFixedProductImage;

[Tags("FixedProductColor")]
[Route("api/FixedProductColor")]
[ApiController]
public class DeleteFixedProductImageEndpoint(ISender sender) : ControllerBase
{

    [Authorize]
    [HttpDelete("DeleteImage/{ImageId:int}", Name = "DeleteImageFromProductColor")]
    public async Task<IActionResult> DeleteImage(int ImageId, CancellationToken cancellationToken)
    {
        var request = new DeleteFixedProductImageRequestDto { ImageId = ImageId };

        var isSuccess = await sender.Send(request, cancellationToken);

        if (isSuccess)
        {
            return Ok(new { Message = "Image deleted successfully." });
        }

        return BadRequest("Failed to delete image. It may not exist.");
    }
}