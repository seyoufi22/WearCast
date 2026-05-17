namespace WearCast.Api.Features.DesignedProductManagement.DesignedProductReviews.UpsertDesignedProductReview
{
    public record UpsertDesignedProductReviewRequest(
        int Rating,
        string? Comment,
        int DesignedProductId
    ) : IRequest<Result<UpsertDesignedProductReviewResponse>>;
}
