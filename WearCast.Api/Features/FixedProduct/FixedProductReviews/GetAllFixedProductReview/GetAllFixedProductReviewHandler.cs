using WearCast.Api.Common.Helper;
using WearCast.Api.Common.Views;
namespace WearCast.Api.Features.FixedProduct.FixedProductReviews.GetAllFixedProductReview;

public class GetAllFixedProductReviewHandler(
    ApplicationDbContext context
    ) : IRequestHandler<GetAllFixedProductReviewRequest, Result<PagingViewModel<GetAllFixedProductReviewResponse>>>
{
    public async Task<Result<PagingViewModel<GetAllFixedProductReviewResponse>>> Handle(GetAllFixedProductReviewRequest request, CancellationToken cancellationToken)
    {
        var productExists = await context.FixedProducts
            .AnyAsync(p => p.Id == request.FixedProductId, cancellationToken);

        if (!productExists)
        {
            return Result.Failure<PagingViewModel<GetAllFixedProductReviewResponse>>(
                new Error("FixedProduct.NotFound", "Product not found.", StatusCodes.Status404NotFound));
        }

        var query = context.FixedProductReviews
            .Where(r => r.FixedProductId == request.FixedProductId)
            .OrderByDescending(r => r.CreatedOn)
            .Select(r => new GetAllFixedProductReviewResponse(
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