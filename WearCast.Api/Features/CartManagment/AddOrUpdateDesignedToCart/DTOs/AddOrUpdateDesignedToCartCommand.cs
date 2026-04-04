namespace WearCast.Api.Features.CartManagment.AddOrUpdateDesignedToCart.DTOs;

public record AddOrUpdateDesignedToCartCommand(
    AddOrUpdateDesignedToCartRequest Request,
    int CustomerId) : IRequest<Result>;
public class AddOrUpdateDesignedToCartValidator : AbstractValidator<AddOrUpdateDesignedToCartCommand>
{
    public AddOrUpdateDesignedToCartValidator()
    {
        RuleFor(x => x.Request.Quantity).NotEqual(0)
            .WithMessage("Quantity change cannot be zero.");

        RuleFor(x => x.Request.DesignId).GreaterThan(0);

        RuleFor(x => x.Request.Size).IsInEnum()
            .WithMessage("The selected size is invalid.");
    }
}
