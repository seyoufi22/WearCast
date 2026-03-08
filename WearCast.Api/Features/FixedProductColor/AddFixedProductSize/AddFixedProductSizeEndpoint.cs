using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WearCast.Api.Features.FixedProductSize.AddFixedProductSize.DTOs;

namespace WearCast.Api.Features.FixedProductSize.AddFixedProductSize;

[Tags("FixedProductColor")]
[Route("api/FixedProductColor")]
[ApiController]
public class AddFixedProductSizeEndpoint(ISender sender) : ControllerBase
{
    [Authorize]
    [HttpPost("AddSize")]
    public async Task<IActionResult> AddSize([FromBody] AddFixedProductSizeRequestDto request, CancellationToken cancellationToken)
    {
        var isSuccess = await sender.Send(request, cancellationToken);

        if (isSuccess)
        {
            return Ok(new { Message = "Size added successfully to the product color." });
        }

        return NotFound(new { Message = "Product color not found." });
    }
}