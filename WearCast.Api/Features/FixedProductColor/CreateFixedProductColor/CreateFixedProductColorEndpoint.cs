using WearCast.Api.Features.FixedProductColor.CreateFixedProductColor.DTOs;

namespace WearCast.Api.Features.FixedProductColor.CreateFixedProductColor;

[Tags("FixedProductColor")]
[Route("api/FixedProductColor")]
[ApiController]
public class CreateFixedProductColorEndpoint(ISender sender) : ControllerBase
{
    private readonly ISender _sender = sender;
    [Authorize]
    [HttpPost("CreateProductColor")]
    [Consumes("multipart/form-data")]
    public async Task<ActionResult<int>> Create(
        [FromForm] CreateFixedProductColorRequestDto request,
        CancellationToken cancellationToken)
    {
        var result = await _sender.Send(request, cancellationToken);
        return Created(string.Empty, result);
    }
}