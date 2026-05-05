namespace WearCast.Api.Features.FixedProduct.FixedProductReviews.UpsertFixedProductReview;

public record UpsertFixedProductReviewRequest(
    int Rating,
    string? Comment,
    int FixedProductId
) : IRequest<Result<UpsertFixedProductReviewResponse>>;