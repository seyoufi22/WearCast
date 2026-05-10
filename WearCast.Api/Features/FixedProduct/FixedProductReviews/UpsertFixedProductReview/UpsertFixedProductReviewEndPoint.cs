namespace WearCast.Api.Features.FixedProduct.FixedProductReviews.UpsertFixedProductReview;

[ApiController]
[Route("api/fixed-products")]
[Tags("Fixed Product Review")]
[Authorize(Roles = DefaultRoles.Customer)]
public class UpsertFixedProductReviewEndPoint(IMediator mediator) : ControllerBase
{
    private readonly IMediator _mediator = mediator;

    [HttpPost("{productId}/reviews")]
    public async Task<IActionResult> Upsert([FromRoute] int productId, [FromBody] UpsertFixedProductReviewBody body, CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(new UpsertFixedProductReviewRequest(
            body.Rating,
            body.Comment,
            productId
            ), cancellationToken);

        return result.ToResponse();
    }
}

public record UpsertFixedProductReviewBody(int Rating, string? Comment);