namespace WearCast.Api.Features.FixedProduct.FixedProductReviews.DeleteFixedProductReview;

[ApiController]
[Route("api/fixed-product-reviews")]
[Tags("Fixed Product Review")]
[Authorize(Roles = $"{DefaultRoles.Customer},{DefaultRoles.CatalogAdmin},{DefaultRoles.CustomerServiceAdmin},{DefaultRoles.SuperAdmin}")]
public class DeleteFixedProductReviewEndPoint(IMediator mediator) : ControllerBase
{
    private readonly IMediator _mediator = mediator;

    [HttpDelete("{reviewId}")]
    public async Task<IActionResult> Delete([FromRoute] int reviewId, CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(new DeleteFixedProductReviewRequest(reviewId), cancellationToken);

        return result.ToResponse();
    }
}