namespace WearCast.Api.Features.FixedProduct.FixedProductReviews.DeleteFixedProductReview;

public record DeleteFixedProductReviewRequest(int ReviewId) : IRequest<Result>;