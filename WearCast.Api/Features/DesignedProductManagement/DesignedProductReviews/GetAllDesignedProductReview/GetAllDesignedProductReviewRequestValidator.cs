namespace WearCast.Api.Features.DesignedProductManagement.DesignedProductReviews.GetAllDesignedProductReview
{
    public class GetAllDesignedProductReviewRequestValidator : AbstractValidator<GetAllDesignedProductReviewRequest>
    {
        public GetAllDesignedProductReviewRequestValidator()
        {
            RuleFor(x => x.DesignedProductId)
                .GreaterThan(0).WithMessage("Invalid Product Id");
        }
    }
}
