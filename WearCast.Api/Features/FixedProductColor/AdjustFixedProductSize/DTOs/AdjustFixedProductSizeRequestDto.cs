namespace WearCast.Api.Features.FixedProductColor.AdjustFixedProductSize.DTOs;

public class AdjustFixedProductSizeRequestDto : IRequest<bool>
{
    public int ProductColorId { get; set; }
    public Size Size { get; set; }
    public int Amount { get; set; }
}

public class AdjustFixedProductSizeValidator : AbstractValidator<AdjustFixedProductSizeRequestDto>
{
    private readonly ApplicationDbContext _context;

    public AdjustFixedProductSizeValidator(ApplicationDbContext context)
    {
        _context = context;

        RuleFor(x => x.ProductColorId)
            .GreaterThan(0).WithMessage("ProductColorId must be greater than 0.");

        RuleFor(x => x.Size)
            .IsInEnum().WithMessage("Invalid Size. Please select a valid size from the list.")
            .MustAsync(async (dto, size, cancellationToken) => await SizeExistsForColorAsync(dto, size, cancellationToken))
            .WithMessage("This Size does not exist for the selected Product Color.");

        RuleFor(x => x.Amount)
            .NotEqual(0).WithMessage("Amount cannot be zero.")
            .ExclusiveBetween(-10000, 10000).WithMessage("Adjustment amount is outside the allowed range.");
    }

    private async Task<bool> SizeExistsForColorAsync(AdjustFixedProductSizeRequestDto dto, Size size, CancellationToken cancellationToken)
    {
        return await _context.FixedProductSizes
            .AnyAsync(s => s.ProductColorId == dto.ProductColorId && s.Size == size, cancellationToken);
    }
}