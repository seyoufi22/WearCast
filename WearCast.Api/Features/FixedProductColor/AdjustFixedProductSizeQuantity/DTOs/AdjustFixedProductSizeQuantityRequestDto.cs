namespace WearCast.Api.Features.FixedProductSize.AdjustFixedProductSizeQuantity.DTOs;

public class AdjustFixedProductSizeQuantityRequestDto : IRequest<bool>
{
    public int ColorId { get; set; }
    public Size Size { get; set; }
    public int Quantity { get; set; }
}

public class AdjustFixedProductSizeQuantityValidator : AbstractValidator<AdjustFixedProductSizeQuantityRequestDto>
{
    private readonly IRepository<Entities.FixedProduct.FixedProductColor> _colorRepo;

    public AdjustFixedProductSizeQuantityValidator(IRepository<Entities.FixedProduct.FixedProductColor> colorRepo)
    {
        _colorRepo = colorRepo;

        RuleFor(x => x.ColorId).GreaterThan(0);

        RuleFor(x => x.Size)
            .IsInEnum().WithMessage("Invalid size. Please select a valid size.")
            .MustAsync(async (dto, sizeEnum, cancellationToken) =>
            {
                var currentColor = await _colorRepo.Get()
                    .Include(c => c.Sizes)
                    .FirstOrDefaultAsync(c => c.Id == dto.ColorId, cancellationToken);

                if (currentColor == null) return true;

                return currentColor.Sizes.Any(s => s.Size == sizeEnum);
            })
            .WithMessage("This size does not exist for the selected color. Please add it first.");

        RuleFor(x => x)
            .MustAsync(async (dto, cancellationToken) =>
            {
                var currentColor = await _colorRepo.Get()
                    .Include(c => c.Sizes)
                    .FirstOrDefaultAsync(c => c.Id == dto.ColorId, cancellationToken);

                if (currentColor == null) return true;

                var existingSize = currentColor.Sizes.FirstOrDefault(s => s.Size == dto.Size);
                if (existingSize == null) return true;

                var expectedQuantity = existingSize.Quantity + dto.Quantity;

                return expectedQuantity >= 0;
            })
            .WithName("Quantity")
            .WithMessage("Not enough stock.");
    }
}