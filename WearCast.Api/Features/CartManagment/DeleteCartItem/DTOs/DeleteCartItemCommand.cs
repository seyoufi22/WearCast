namespace WearCast.Api.Features.CartManagment.DeleteCartItem.DTOs;

public record DeleteCartItemCommand (
    int ColorId,
    int CustomerId) : IRequest<bool>;
public class DeleteCartItemValidator : AbstractValidator<DeleteCartItemCommand>
{
    private readonly ApplicationDbContext _context;

    public DeleteCartItemValidator(ApplicationDbContext context)
    {
        _context = context;

        ClassLevelCascadeMode = CascadeMode.Stop;
        RuleLevelCascadeMode = CascadeMode.Stop;

        RuleFor(x => x.ColorId)
            .GreaterThan(0).WithMessage("Color ID must be greater than 0.");

        RuleFor(x => x)
            .MustAsync(async (cmd, cancellationToken) =>
            {
                return await _context.CartItems
                    .AnyAsync(c => c.CustomerId == cmd.CustomerId
                                && c.ColorId == cmd.ColorId,
                              cancellationToken);
            })
            .OverridePropertyName("CartItem") 
            .WithMessage("This item does not exist in your cart.");
    }
}