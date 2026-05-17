namespace WearCast.Api.Features.DesignedProductManagement.DesignedProductReviews.GetMyDesignedProductReview
{
    public record GetMyDesignedProductReviewRequest(int DesignedProductId) : IRequest<Result<GetMyDesignedProductReviewResponse>>;
}
