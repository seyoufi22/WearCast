using WearCast.Api.Features.FixedProductColor.AddFixedProductImage.DTOs;

namespace WearCast.Api.Features.FixedProductColor.AddFixedProductImage;

[Tags("FixedProductColor")]
[Route("api/FixedProductColor")]
[ApiController]
public class AddFixedProductImageEndpoint(IMediator mediator) : ControllerBase
{
    private readonly IMediator _mediator = mediator;

    [Authorize]
    [HttpPost("AddImage")]
    [Consumes("multipart/form-data")] 
    public async Task<IActionResult> AddImage([FromForm] AddFixedProductImageRequestDto request)
    {
        var result = await _mediator.Send(request);

        if (result)
        {
            return Ok(new { Message = "Image uploaded successfully."});
        }

        return BadRequest("Failed to add image. Invalid file or ProductColor does not exist.");
    }
}