using WearCast.Api.Features.FixedProductColor.CreateFixedProductSize.DTOs;

namespace WearCast.Api.Features.FixedProductColor.CreateFixedProductSize;

[Tags("FixedProductColor")]
[Route("api/FixedProductColor")]
[ApiController]
public class CreateFixedProductSizeEndpoint : ControllerBase
{
    private readonly IMediator _mediator;

    public CreateFixedProductSizeEndpoint(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpPost("AddProductSize")]
    public async Task<IActionResult> Create([FromBody] CreateFixedProductSizeRequestDto request)
    {
        var isSuccess = await _mediator.Send(request);

        if (isSuccess)
        {
            return Ok(new { Message = "Product Size added successfully." });
        }

        return BadRequest("Failed to add size. The ProductColor may not exist.");
    }
}