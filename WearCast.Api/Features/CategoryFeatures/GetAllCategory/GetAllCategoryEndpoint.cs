using WearCast.Api.Features.CategoryFeatures.GetAllCategory.DTOs;

namespace WearCast.Api.Features.CategoryFeatures.GetAllCategory;

[Tags("Category")]
[Route("api/Category")]
[ApiController]
public class GetAllCategoryEndPoint(ISender sender) : ControllerBase
{
    private readonly ISender _sender = sender;

    [HttpGet("GetAllCategories", Name = "GetAllCategories")]
    public async Task<ActionResult<List<GetAllCategoryResponseDto>>> GetAll(CancellationToken cancellationToken)
    {
        var result = await _sender.Send(new GetAllCategoryRequestDto(), cancellationToken);

        return Ok(result);
    }
}