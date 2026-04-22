namespace WearCast.Api.Features.DesignedProductManagement.DesignedProductReviews.GetAllDesignedProductReview
{
    [ApiController]
    [Route("api/designed-products")]
    [Tags("Designed Product Review")]
    [Authorize(Roles = $"{DefaultRoles.Customer},{DefaultRoles.SuperAdmin},{DefaultRoles.FactoryManager}")]
    public class GetAllDesignedProductReviewEndPoint(IMediator mediator) : ControllerBase
    {
        private readonly IMediator _mediator = mediator;

        [HttpGet("{productId}/reviews")]
        public async Task<IActionResult> GetAllReviews(
            [FromRoute] int productId,
            [FromQuery] int pageIndex = 1,
            [FromQuery] int pageSize = 10, CancellationToken cancellationToken = default)
        {
            var query = new GetAllDesignedProductReviewRequest(productId, pageIndex, pageSize);

            var result = await _mediator.Send(query, cancellationToken);

            return result.ToResponse();
        }
    }
}
