namespace WearCast.Api.Features.DesignedProductManagement.DesignedProductReviews.GetMyDesignedProductReview
{
    [ApiController]
    [Route("api/designed-products")]
    [Tags("Designed Product Review")]
    [Authorize(Roles = DefaultRoles.Customer)]
    public class GetMyDesignedProductReviewEndPoint(IMediator mediator) : ControllerBase
    {
        private readonly IMediator _mediator = mediator;
        [HttpGet("{productId}/my-review")]
        public async Task<IActionResult> GetMyReview([FromRoute] int productId, CancellationToken cancellationToken)
        {
            var result = await _mediator.Send(new GetMyDesignedProductReviewRequest(productId), cancellationToken);

            return result.ToResponse();
        }
    }
}
