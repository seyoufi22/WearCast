using WearCast.Api.Features.CategoryFeatures.UpdateCategory.DTOs;

namespace WearCast.Api.Features.CategoryFeatures.UpdateCategory;

[Tags("Category")]
[Route("api/Category")]
[ApiController]
public class UpdateCategoryEndPoint(ISender sender) : ControllerBase
{
    [Authorize]
    [HttpPut("UpdateCategory/{id:int}", Name = "UpdateCategory")]
    [Consumes("multipart/form-data")]
    [ProducesResponseType(204)]
    [ProducesResponseType(404)]
    public async Task<IActionResult> Update(int id, [FromForm] UpdateCategoryRequest request, CancellationToken cancellationToken)
    {
        var result = await sender.Send(new UpdateCategoryRequestDto(id, request.Name, request.Image));
        if (!result)
            return NotFound();
        return NoContent();
    }
}

public record UpdateCategoryRequest(string Name, IFormFile? Image);