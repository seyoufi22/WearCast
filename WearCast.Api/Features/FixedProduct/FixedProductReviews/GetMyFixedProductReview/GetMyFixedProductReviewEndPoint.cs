namespace WearCast.Api.Features.FixedProduct.FixedProductReviews.GetMyFixedProductReview;

[ApiController]
[Route("api/fixed-products")]
[Tags("Fixed Product Review")]
[Authorize(Roles = DefaultRoles.Customer)]
public class GetMyFixedProductReviewEndPoint(IMediator mediator) : ControllerBase
{
    private readonly IMediator _mediator = mediator;

    [HttpGet("{productId}/my-review")]
    public async Task<IActionResult> GetMyReview([FromRoute] int productId, CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(new GetMyFixedProductReviewRequest(productId), cancellationToken);

        return result.ToResponse();
    }
}