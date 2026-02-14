using WearCast.Api.Features.CategoryFeatures.Queries;

namespace WearCast.Api.Features.CategoryFeatures
{
    [ApiController]
    [Route("api/[controller]")]
    public class CategoryController : ControllerBase
    {
        private readonly ISender _sender;

        public CategoryController(ISender sender)
        {
            _sender = sender;
        }
        [HttpGet]
        [Route("All", Name = "GetAllCategories")]
        public async Task<ActionResult<List<GetAllCategory.CategoryResponse>>> GetAll()
        {

            var result = await _sender.Send(new GetAllCategory.GetCategoriesQuery());
            return Ok(result);
        }
    }
}
