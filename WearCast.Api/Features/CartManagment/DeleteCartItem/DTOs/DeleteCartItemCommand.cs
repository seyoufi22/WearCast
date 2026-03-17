namespace WearCast.Api.Features.CartManagment.DeleteCartItem.DTOs;

public record DeleteCartItemCommand (
    int CartItemId,
    int CustomerId) : IRequest<Result>;
public class DeleteCartItemValidator : AbstractValidator<DeleteCartItemCommand>
{
    public DeleteCartItemValidator()
    {
        RuleFor(x => x.CartItemId)
            .GreaterThan(0).WithMessage("CartItem ID must be greater than 0.");

    }
}