using WearCast.Api.Features.CartManagement.AddOrUpdateCartItem.DTOs;

namespace WearCast.Api.Features.CartManagment.AddOrUpdateCartItem.DTOs;

public record AddOrUpdateCartItemCommand(
    AddOrUpdateCartItemRequest Request,
    int CustomerId) : IRequest<Result>;
public class AddOrUpdateCartItemValidator : AbstractValidator<AddOrUpdateCartItemCommand>
{
    public AddOrUpdateCartItemValidator()
    {
        RuleFor(x => x.Request.Quantity).NotEqual(0)
            .WithMessage("Quantity change cannot be zero.");

        RuleFor(x => x.Request.ColorId).GreaterThan(0);

        RuleFor(x => x.Request.Size).IsInEnum()
            .WithMessage("The selected size is invalid.");
    }
}
