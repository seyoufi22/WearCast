namespace WearCast.Api.Features.FixedProduct.FixedProductReviews.GetMyFixedProductReview;

public class GetMyFixedProductReviewRequestValidator : AbstractValidator<GetMyFixedProductReviewRequest>
{
    public GetMyFixedProductReviewRequestValidator()
    {
        RuleFor(x => x.FixedProductId)
            .GreaterThan(0).WithMessage("Invalid Product ID.");
    }
}