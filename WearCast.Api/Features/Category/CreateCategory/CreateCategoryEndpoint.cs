using WearCast.Api.Features.Category.CreateCategory.DTOs;

namespace WearCast.Api.Features.Category.CreateCategory;

[Tags("Category")]
[Route("api/Category")]
[ApiController]
public class CreateCategoryEndPoint(ISender sender) : ControllerBase
{
    [Authorize]
    [HttpPost("CreateCategory", Name = "AddCategory")]
    [Consumes("multipart/form-data")]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Create([FromForm] CreateCategoryRequestDto request, CancellationToken cancellationToken)
    {
        var category = await sender.Send(request);
        return CreatedAtRoute("GetCategoryById", new { id = category.Id }, category);
    }
}