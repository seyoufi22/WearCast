namespace WearCast.Api.Features.DesignedProductManagement.DesignedProductReviews.GetMyDesignedProductReview
{
    public record GetMyDesignedProductReviewResponse(
        int Id,
        int Rating,
        string? Comment,
        DateTime CreatedAt
        );
}
