namespace WearCast.Api.Features.CategoryFeatures.GetAllCategory;

[Tags("Category")]
[Route("api/Category")]
[ApiController]
public class GetAllCategoryEndPoint(ISender sender) : ControllerBase
{
    private readonly ISender _sender = sender;

    [HttpGet("GetAllCategories", Name = "GetAllCategories")]
    public async Task<ActionResult<List<CategoryResponse>>> GetAll(CancellationToken cancellationToken)
    {
        var result = await _sender.Send(new GetCategoriesQuery(), cancellationToken);

        return Ok(result);
    }
}