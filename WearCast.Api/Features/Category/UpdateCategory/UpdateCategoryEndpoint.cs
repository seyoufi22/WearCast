using WearCast.Api.Features.Category.UpdateCategory.DTOs;

namespace WearCast.Api.Features.Category.UpdateCategory;

[Tags("Category")]
[Route("api/Category")]
[ApiController]
public class UpdateCategoryEndPoint(ISender sender) : ControllerBase
{
    [Authorize(Roles = $"{DefaultRoles.SuperAdmin},{DefaultRoles.CatalogAdmin}")]
    [HttpPut("UpdateCategory", Name = "UpdateCategory")]
    [Consumes("multipart/form-data")]
    [ProducesResponseType(204)]
    [ProducesResponseType(404)]
    public async Task<IActionResult> Update( [FromForm] UpdateCategoryRequestDto request, CancellationToken cancellationToken)
    {
        var result = await sender.Send(request);
        if (result.IsFailure)
        {
            return StatusCode(result.Error.StatusCode ?? StatusCodes.Status400BadRequest, result.Error);
        }
        return NoContent();
    }
}
