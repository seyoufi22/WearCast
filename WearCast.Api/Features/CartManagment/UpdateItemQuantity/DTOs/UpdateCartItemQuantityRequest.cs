namespace WearCast.Api.Features.CartManagment.UpdateCartItemQuantity.DTOs;

public record UpdateCartItemQuantityRequest(
    int CartItemId,
    Size Size,
    int QuantityChange // Send 1 for (++), and -1 for (--)
) : IRequest<Result>;

public class UpdateCartItemQuantityValidator : AbstractValidator<UpdateCartItemQuantityRequest>
{
    public UpdateCartItemQuantityValidator()
    {
        // 1. Check if CartItemId is valid
        RuleFor(x => x.CartItemId)
            .GreaterThan(0)
            .WithMessage("CartItem ID must be greater than 0.");

        // 2. Check if the Size enum is valid
        RuleFor(x => x.Size)
            .IsInEnum()
            .WithMessage("The selected size is invalid.");

        // 3. Check if QuantityChange is EXACTLY 1 or -1
        RuleFor(x => x.QuantityChange)
            .Must(change => change == 1 || change == -1)
            .WithMessage("QuantityChange must be exactly 1 for increment (++) or -1 for decrement (--).");
    }
}