
namespace WearCast.Api.Features.DesignedProductManagement.DesignedProductReviews.UpsertDesignedProductReview
{
    [ApiController]
    [Route("api/designed-products")]
    [Tags("Designed Product Review")]
    [Authorize(Roles = DefaultRoles.Customer)]
    public class UpsertDesignedProductReviewEndPoint(IMediator mediator) : ControllerBase
    {
        private readonly IMediator _mediator = mediator;

        [HttpPost("{productId}/reviews")]
        public async Task<IActionResult> Upsert([FromRoute] int productId, [FromBody] UpsertDesignedProductReviewBody body, CancellationToken cancellationToken)
        {
            var result = await _mediator.Send(new UpsertDesignedProductReviewRequest(
                body.Rating,
                body.Comment,
                productId
                ), cancellationToken);

            return result.ToResponse();
        }
    }
    public record UpsertDesignedProductReviewBody(int Rating, string? Comment);
}
