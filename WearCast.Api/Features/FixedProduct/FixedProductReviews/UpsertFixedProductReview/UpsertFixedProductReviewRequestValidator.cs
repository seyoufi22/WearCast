namespace WearCast.Api.Features.FixedProduct.FixedProductReviews.UpsertFixedProductReview;

public class UpsertFixedProductReviewRequestValidator : AbstractValidator<UpsertFixedProductReviewRequest>
{
    public UpsertFixedProductReviewRequestValidator()
    {
        RuleFor(x => x.FixedProductId)
            .GreaterThan(0).WithMessage("Invalid Product ID.");

        RuleFor(x => x.Rating)
            .InclusiveBetween(1, 5).WithMessage("Rating must be between 1 and 5 stars.");

        RuleFor(x => x.Comment)
            .MaximumLength(500).WithMessage("Comment cannot exceed 500 characters.")
            .When(x => !string.IsNullOrWhiteSpace(x.Comment));
    }
}