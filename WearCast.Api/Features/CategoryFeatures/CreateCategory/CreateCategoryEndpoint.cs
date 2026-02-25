using WearCast.Api.Features.CategoryFeatures.CreateCategory.DTOs;

namespace WearCast.Api.Features.CategoryFeatures.CreateCategory;

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
    public async Task<IActionResult> Create([FromForm] CreateCategoryResponseDto request, CancellationToken cancellationToken)
    {
        var category = await sender.Send(new CreateCategoryRequestDto(request.Name, request.Image));
        return CreatedAtRoute("GetCategoryById", new { id = category.Id }, category);
    }
}