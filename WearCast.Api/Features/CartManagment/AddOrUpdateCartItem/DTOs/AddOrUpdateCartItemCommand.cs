using WearCast.Api.Features.CartManagement.AddOrUpdateCartItem.DTOs;

namespace WearCast.Api.Features.CartManagment.AddOrUpdateCartItem.DTOs;

public record AddOrUpdateCartItemCommand(
    AddOrUpdateCartItemRequest Request,
    int CustomerId) : IRequest<bool>;
public class AddOrUpdateCartItemValidator : AbstractValidator<AddOrUpdateCartItemCommand>
{
    private readonly IRepository<Entities.FixedProduct.FixedProductColor> _colorRepo;

    public AddOrUpdateCartItemValidator(IRepository<Entities.FixedProduct.FixedProductColor> colorRepo)
    {
        _colorRepo = colorRepo;

        ClassLevelCascadeMode = CascadeMode.Stop;

        RuleFor(x => x.CustomerId)
            .GreaterThan(0).WithMessage("Invalid Customer ID.");

        RuleFor(x => x.Request.Quantity)
            .NotEqual(0).WithMessage("Quantity change cannot be zero.");

        RuleFor(x => x.Request.ColorId)
            .GreaterThan(0).WithMessage("Color ID must be greater than 0.")
            .MustAsync(async (id, cancellation) =>
                await _colorRepo.Get().AnyAsync(c => c.Id == id && !c.IsDeleted, cancellation))
            .WithMessage("The specified color does not exist or is unavailable.");

        RuleFor(x => x.Request.Size)
            .NotEmpty().WithMessage("Size is required.")
            .IsInEnum().WithMessage("The selected size is invalid.");

        RuleFor(x => x)
            .MustAsync(async (cmd, cancellation) =>
            {
                return await _colorRepo.Get()
                    .AnyAsync(c => c.Id == cmd.Request.ColorId
                                && c.Sizes.Any(s => s.Size == cmd.Request.Size), cancellation);
            })
            .OverridePropertyName("Request.Size")
            .WithMessage("This size is not available for the selected color.");
    }
}
