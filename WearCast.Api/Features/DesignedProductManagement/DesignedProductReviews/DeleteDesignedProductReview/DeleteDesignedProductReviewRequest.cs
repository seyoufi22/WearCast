namespace WearCast.Api.Features.DesignedProductManagement.DesignedProductReviews.DeleteDesignedProductReview
{
    public record DeleteDesignedProductReviewRequest(int ReviewId) : IRequest<Result>;
}
