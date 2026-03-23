namespace WearCast.Api.Features.CartManagment.AddOrUpdateFixedColorToCart.DTOs;

public record AddOrUpdateFixedColorToCartCommand(
    AddOrUpdateFixedColorToCartRequest Request,
    int CustomerId) : IRequest<Result>;
public class AddOrUpdateFixedColorToCartValidator : AbstractValidator<AddOrUpdateFixedColorToCartCommand>
{
    public AddOrUpdateFixedColorToCartValidator()
    {
        RuleFor(x => x.Request.Quantity).NotEqual(0)
            .WithMessage("Quantity change cannot be zero.");

        RuleFor(x => x.Request.ColorId).GreaterThan(0);

        RuleFor(x => x.Request.Size).IsInEnum()
            .WithMessage("The selected size is invalid.");
    }
}
