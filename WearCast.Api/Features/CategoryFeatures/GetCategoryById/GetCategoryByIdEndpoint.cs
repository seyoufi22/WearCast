using WearCast.Api.Features.CategoryFeatures.GetCategoryById.DTOs;

namespace WearCast.Api.Features.CategoryFeatures.GetCategoryById;

[Tags("Category")]
[Route("api/Category")]
[ApiController]
public class GetCategoryByIdEndPoint(ISender sender) : ControllerBase
{
    private readonly ISender _sender = sender;

    [HttpGet("GetCategoryById/{id:int}", Name = "GetCategoryById")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<CategoryResponse>> GetById(int id, CancellationToken cancellationToken)
    {
        if (id <= 0)
            return BadRequest("Invalid category ID.");

        var result = await _sender.Send(new GetCategoryByIdRequestDto(id), cancellationToken);

        if (result == null)
            return NotFound();

        return Ok(result);
    }
}