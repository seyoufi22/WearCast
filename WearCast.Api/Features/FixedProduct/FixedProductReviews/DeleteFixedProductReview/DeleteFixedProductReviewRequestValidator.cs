namespace WearCast.Api.Features.FixedProduct.FixedProductReviews.DeleteFixedProductReview;

public class DeleteFixedProductReviewRequestValidator : AbstractValidator<DeleteFixedProductReviewRequest>
{
    public DeleteFixedProductReviewRequestValidator()
    {
        RuleFor(x => x.ReviewId)
            .GreaterThan(0).WithMessage("Invalid Review ID.");
    }
}