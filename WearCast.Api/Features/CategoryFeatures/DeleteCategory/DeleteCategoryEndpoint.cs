namespace WearCast.Api.Features.CategoryFeatures.DeleteCategory;

[Tags("Category")]
[Route("api/Category")]
[ApiController]
public class DeleteCategoryEndPoint(ISender sender) : ControllerBase
{
    [Authorize]
    [HttpDelete("DeleteCategory/{id:int}", Name = "DeleteCategory")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Delete(int id, CancellationToken cancellationToken)
    {
        var result = await sender.Send(new DeleteCategoryCommand(id), cancellationToken);
        if (!result)
            return NotFound();
        return NoContent();
    }
}