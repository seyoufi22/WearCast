namespace WearCast.Api.Features.FixedProduct.FixedProductReviews.GetAllFixedProductReview;

[ApiController]
[Route("api/fixed-products")] 
[Tags("Fixed Product Review")]
public class GetAllFixedProductReviewEndPoint(IMediator mediator) : ControllerBase
{
    private readonly IMediator _mediator = mediator;

    [HttpGet("{productId}/reviews")]
    public async Task<IActionResult> GetAllReviews(
        [FromRoute] int productId,
        [FromQuery] int pageIndex = 1,
        [FromQuery] int pageSize = 10, CancellationToken cancellationToken = default)
    {
        var query = new GetAllFixedProductReviewRequest(productId, pageIndex, pageSize);

        var result = await _mediator.Send(query, cancellationToken);

        return result.ToResponse();
    }
}