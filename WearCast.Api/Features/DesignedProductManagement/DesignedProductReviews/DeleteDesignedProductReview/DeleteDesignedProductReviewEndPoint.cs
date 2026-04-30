namespace WearCast.Api.Features.DesignedProductManagement.DesignedProductReviews.DeleteDesignedProductReview
{
    [ApiController]
    [Route("api/designed-product-reviews")]
    [Tags("Designed Product Review")]
    [Authorize(Roles = $"{DefaultRoles.Customer},{DefaultRoles.CatalogAdmin},{DefaultRoles.CustomerServiceAdmin},{DefaultRoles.SuperAdmin}")]
    public class DeleteDesignedProductReviewEndPoint(IMediator mediator) : ControllerBase
    {
        private readonly IMediator _mediator = mediator;

        [HttpDelete("{reviewId}")]
        public async Task<IActionResult> Delete([FromRoute] int reviewId, CancellationToken cancellationToken)
        {
            var result = await _mediator.Send(new DeleteDesignedProductReviewRequest(reviewId), cancellationToken);

            return result.ToResponse();
        }
    }
}
