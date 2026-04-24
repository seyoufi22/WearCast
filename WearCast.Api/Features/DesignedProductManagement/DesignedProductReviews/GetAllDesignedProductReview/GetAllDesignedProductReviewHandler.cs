using WearCast.Api.Common.Helper;
using WearCast.Api.Common.Views;
// تأكد من الـ Namespaces الخاصة بالـ Errors والـ Entities

namespace WearCast.Api.Features.DesignedProductManagement.DesignedProductReviews.GetAllDesignedProductReview
{
    public class GetAllDesignedProductReviewHandler(
        ApplicationDbContext context
        ) : IRequestHandler<GetAllDesignedProductReviewRequest, Result<PagingViewModel<GetAllDesignedProductReviewResponse>>>
    {
        public async Task<Result<PagingViewModel<GetAllDesignedProductReviewResponse>>> Handle(GetAllDesignedProductReviewRequest request, CancellationToken cancellationToken)
        {
            var productExists = await context.DesignedProducts
                .AnyAsync(p => p.Id == request.DesignedProductId, cancellationToken);

            if (!productExists)
            {
                return Result.Failure<PagingViewModel<GetAllDesignedProductReviewResponse>>(DesignedProductErrors.ProductNotFound);
            }

            var query = context.DesignedProductReviews
                .Where(r => r.DesignedProductId == request.DesignedProductId)
                .OrderByDescending(r => r.CreatedOn)
                .Select(r => new GetAllDesignedProductReviewResponse(
                    r.Id,
                    r.Customer.Id,
                    $"{r.Customer.ApplicationUser.FirstName} {r.Customer.ApplicationUser.LastName}",
                    r.Customer.ProfileImageUrl,
                    r.Rating,
                    r.Comment,
                    r.CreatedOn
                ));

            var pagedResult = await PagingHelper.CreateAsync(query, request.PageIndex, request.PageSize);

            return Result.Success(pagedResult);
        }
    }
}