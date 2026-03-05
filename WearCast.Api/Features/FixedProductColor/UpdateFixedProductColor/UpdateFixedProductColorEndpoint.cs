using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WearCast.Api.Features.FixedProductColor.UpdateFixedProductColor.DTOs;

namespace WearCast.Api.Features.FixedProductColor.UpdateFixedProductColor;

[Tags("FixedProductColor")]
[Route("api/FixedProductColor")]
[ApiController]
public class UpdateFixedProductColorEndpoint(ISender sender) : ControllerBase
{
    [Authorize]
    [HttpPut("UpdateColor")]
    [Consumes("multipart/form-data")]
    public async Task<IActionResult> UpdateColor([FromForm] UpdateFixedProductColorRequestDto request, CancellationToken cancellationToken)
    {
        var isSuccess = await sender.Send(request, cancellationToken);

        if (isSuccess)
        {
            return Ok(new { Message = "Product color updated successfully." });
        }

        return NotFound(new { Message = "Product color not found." });
    }
}