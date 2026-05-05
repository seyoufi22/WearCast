
namespace WearCast.Api.Features.FixedProduct.FixedProductReviews.GetMyFixedProductReview;

public record GetMyFixedProductReviewRequest(int FixedProductId) : IRequest<Result<GetMyFixedProductReviewResponse>>;