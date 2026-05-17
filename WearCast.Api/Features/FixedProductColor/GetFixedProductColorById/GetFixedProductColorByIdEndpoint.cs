//using WearCast.Api.Features.FixedProductColor.GetFixedProductColorById.DTOs;

//namespace WearCast.Api.Features.FixedProductColor.GetFixedProductColorById;

//[Tags("FixedProductColor")]
//[Route("api/FixedProductColor")]
//[ApiController]
//public class GetFixedProductColorByIdEndpoint(ISender sender) : ControllerBase
//{
//    private readonly ISender _sender = sender;

//    [HttpGet("GetColorById/{Id:int}")]
//    public async Task<ActionResult<GetFixedProductColorByIdResponseDto>> GetById(int Id, CancellationToken cancellationToken)
//    {

//        var result = await _sender.Send(new GetFixedProductColorByIdRequestDto(Id), cancellationToken);

//        if (result == null)
//            return NotFound(new { Message = $"Product color with ID {Id} was not found." });

//        return Ok(result);
//    }
//}