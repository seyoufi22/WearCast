using WearCast.Api.Features.FixedProductColor.GetAllFixedProductColor.DTOs;
namespace WearCast.Api.Features.FixedProductColor.GetAllFixedProductColor;

[Tags("FixedProductColor")]
[Route("api/FixedProductColor")]
[ApiController]
public class GetAllFixedProductColorEndPoint(ISender sender) : ControllerBase
{
    private readonly ISender _sender = sender;

    [HttpGet("GetAllColorByProductId/{Id:int}")]
    public async Task<ActionResult<List<GetAllFixedProductColorResponseDto>>> GetAll(
         int Id,
        CancellationToken cancellationToken)
    {
        var result = await _sender.Send(new GetAllFixedProductColorRequestDto(Id), cancellationToken);

        return Ok(result);
    }
}