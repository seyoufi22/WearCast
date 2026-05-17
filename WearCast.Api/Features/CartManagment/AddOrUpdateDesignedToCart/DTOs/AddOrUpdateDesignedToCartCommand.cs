namespace WearCast.Api.Features.CartManagment.AddOrUpdateDesignedToCart.DTOs;

public record AddOrUpdateDesignedToCartCommand(
    AddOrUpdateDesignedToCartRequest Request,
    int CustomerId) : IRequest<Result>;
public class AddOrUpdateDesignedToCartValidator : AbstractValidator<AddOrUpdateDesignedToCartCommand>
{
    public AddOrUpdateDesignedToCartValidator()
    {
        RuleFor(x => x.Request.DesignId).GreaterThan(0);

        RuleFor(x => x.Request.Sizes)
            .NotEmpty().WithMessage("At least one size must be provided.")
            .Must(sizes => sizes.Select(s => s.Size).Distinct().Count() == sizes.Count)
            .WithMessage("Duplicate sizes are not allowed in the same request. Please sum the quantities first.");

        RuleForEach(x => x.Request.Sizes).ChildRules(sizes =>
        {
            sizes.RuleFor(s => s.Quantity).NotEqual(0)
                .WithMessage("Quantity change cannot be zero.");

            sizes.RuleFor(s => s.Size).IsInEnum()
                .WithMessage("The selected size is invalid.");
        });
    }
}
