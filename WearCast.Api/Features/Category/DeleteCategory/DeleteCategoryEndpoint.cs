using WearCast.Api.Features.Category.DeleteCategory.DTOs;

namespace WearCast.Api.Features.Category.DeleteCategory;

[Tags("Category")]
[Route("api/Category")]
[ApiController]
public class DeleteCategoryEndPoint(ISender sender) : ControllerBase
{
    [Authorize(Roles = $"{DefaultRoles.SuperAdmin},{DefaultRoles.CatalogAdmin}")]
    [HttpDelete("DeleteCategory/{id:int}", Name = "DeleteCategory")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Delete(int id, CancellationToken cancellationToken)
    {
        var result = await sender.Send(new DeleteCategoryRequestDto(id), cancellationToken);
        if (!result.IsSuccess)
        {
            return StatusCode(result.Error.StatusCode ?? StatusCodes.Status400BadRequest, result.Error);
        }
        return NoContent();
    }
}