using WearCast.Api.Common.Views;

namespace WearCast.Api.Features.DesignedProductManagement.DesignedProductReviews.GetAllDesignedProductReview
{
    public record GetAllDesignedProductReviewRequest(
        int DesignedProductId,
        int PageIndex = 1,
        int PageSize = 10
    ) : IRequest<Result<PagingViewModel<GetAllDesignedProductReviewResponse>>>;
}
