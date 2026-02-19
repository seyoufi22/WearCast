using Microsoft.AspNetCore.Authorization;
using WearCast.Api.Features.CategoryFeatures.Commends;
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
        [HttpGet]
        [Route("{id:int}", Name = "GetCategoryById")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<GetCategoryDetails.CategoryResponse>> GetById(int id)
        {
            if (id <= 0) return BadRequest("Invalid category ID.");
            var result = await _sender.Send(new GetCategoryDetails.GetCategoryByIdQuery(id));

            if (result == null)
                return NotFound();

            return Ok(result);
        }
        [Authorize]
        [HttpDelete]
        [Route("{id:int}", Name = "DeleteCategory")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> Delete(int id)
        {
            if (id <= 0) return BadRequest("Invalid category ID."); 
            bool result = await _sender.Send(new DeleteCategory.DeleteCategoryCommand(id));

            if (!result)
                return NotFound("Category not found.");

            return NoContent();
        }
        [Authorize]
        [HttpPost]
        [Route("AddCategory")]
        [Consumes("multipart/form-data")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Create([FromForm] CreateCategory.CreateCategoryCommand command)
        {
            var category = await _sender.Send(command);

            return CreatedAtRoute("GetCategoryById", new { id = category.Id }, category);
        }
        [Authorize]
        [HttpPut]
        [Route("UpdateCategory")]
        [Consumes("multipart/form-data")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Update([FromForm] UpdateCategory.UpdateCategoryCommand command)
        {
            if(command.Id <=0)
                return BadRequest("Invalid category ID.");
            string result = await _sender.Send(command);

            return NoContent();
        }
    }
}
