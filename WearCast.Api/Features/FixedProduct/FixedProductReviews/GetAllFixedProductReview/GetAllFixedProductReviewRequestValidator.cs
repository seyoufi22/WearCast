namespace WearCast.Api.Features.FixedProduct.FixedProductReviews.GetAllFixedProductReview;

public class GetAllFixedProductReviewRequestValidator : AbstractValidator<GetAllFixedProductReviewRequest>
{
    public GetAllFixedProductReviewRequestValidator()
    {
        RuleFor(x => x.FixedProductId)
            .GreaterThan(0).WithMessage("Invalid Product Id");
    }
}