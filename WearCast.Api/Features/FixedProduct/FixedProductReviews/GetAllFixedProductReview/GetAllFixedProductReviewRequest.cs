using WearCast.Api.Common.Views;
using WearCast.Api.Features.FixedProduct.FixedProductReviews.GetAllFixedProductReview;

namespace WearCast.Api.Features.FixedProduct.FixedProductReviews.GetAllFixedProductReview;

public record GetAllFixedProductReviewRequest(
    int FixedProductId,
    int PageIndex = 1,
    int PageSize = 10
) : IRequest<Result<PagingViewModel<GetAllFixedProductReviewResponse>>>;