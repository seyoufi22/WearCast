using WearCast.Api.Features.FixedProductColor.DeleteFixedProductColor.DTOs;

namespace WearCast.Api.Features.FixedProductColor.DeleteFixedProductColor;

[Tags("FixedProductColor")]
[Route("api/FixedProductColor")]
[ApiController]
public class DeleteFixedProductColorEndpoint(ISender sender) : ControllerBase
{
    [Authorize] 
    [HttpDelete("DeleteColor/{ColorId:int}", Name = "DeleteFixedProductColor")]
    public async Task<IActionResult> DeleteColor(int ColorId, CancellationToken cancellationToken)
    {
        var request = new DeleteFixedProductColorRequestDto { ColorId = ColorId };

        var isSuccess = await sender.Send(request, cancellationToken);

        if (isSuccess)
        {
            return Ok(new { Message = "Product color deleted successfully." });
        }

        return NotFound(new { Message = "Product color not found." });
    }
}