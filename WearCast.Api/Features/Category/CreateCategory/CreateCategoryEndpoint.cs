using WearCast.Api.Features.Category.CreateCategory.DTOs;

namespace WearCast.Api.Features.Category.CreateCategory;

[Tags("Category")]
[Route("api/Category")]
[ApiController]
public class CreateCategoryEndPoint(ISender sender) : ControllerBase
{
    [Authorize(Roles = $"{DefaultRoles.SuperAdmin},{DefaultRoles.CatalogAdmin}")]
    [HttpPost("CreateCategory", Name = "AddCategory")]
    [Consumes("multipart/form-data")]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Create([FromForm] CreateCategoryRequestDto request, CancellationToken cancellationToken)
    {
        var result = await sender.Send(request);
        if (result.IsFailure)
        {
            return StatusCode(result.Error.StatusCode ?? StatusCodes.Status400BadRequest, result.Error);
        }
        return Ok(new { Message = "Category added successfully." });
    }
}