namespace WearCast.Api.Features.FixedProduct.FixedProductReviews.GetMyFixedProductReview;

public record GetMyFixedProductReviewResponse(
    int Id,
    int Rating,
    string? Comment,
    DateTime CreatedAt
);