namespace WearCast.Api.Features.FixedProductColor.CreateFixedProductSize.DTOs;

public class CreateFixedProductSizeRequestDto : IRequest<bool>
{
    public int ProductColorId { get; set; }
    public Size Size { get; set; }
    public int Quantity { get; set; }
}

public class CreateFixedProductSizeValidator : AbstractValidator<CreateFixedProductSizeRequestDto>
{
    private readonly ApplicationDbContext _context;

    public CreateFixedProductSizeValidator(ApplicationDbContext context)
    {
        _context = context;

        RuleFor(x => x.ProductColorId)
            .GreaterThan(0).WithMessage("ProductColorId must be greater than 0.");

        RuleFor(x => x.Size)
            .IsInEnum().WithMessage("Invalid Size. Please select a valid size from the list.")
            .MustAsync(async (dto, size, cancellationToken) => await BeUniqueSizeForColorAsync(dto, size, cancellationToken))
            .WithMessage("This Size is already added to the selected Product Color.");

        RuleFor(x => x.Quantity)
            .GreaterThanOrEqualTo(0).WithMessage("Quantity cannot be negative.")
            .LessThan(10000).WithMessage("Quantity exceeds the maximum allowed limit.");
    }

    private async Task<bool> BeUniqueSizeForColorAsync(CreateFixedProductSizeRequestDto dto, Size size, CancellationToken cancellationToken)
    {
        var exists = await _context.FixedProductSizes
            .AnyAsync(s => s.ProductColorId == dto.ProductColorId && s.Size == size, cancellationToken);

        return !exists;
    }
}