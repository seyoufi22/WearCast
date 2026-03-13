using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;
using WearCast.Api.Common.Repository;
using WearCast.Api.Common.Enums;

namespace WearCast.Api.Features.FixedProductSize.AddFixedProductSize.DTOs;

public class AddFixedProductSizeRequestDto : IRequest<bool>
{
    public int ColorId { get; set; }
    public Size Size { get; set; }
    public int Quantity { get; set; }
}

public class AddFixedProductSizeValidator : AbstractValidator<AddFixedProductSizeRequestDto>
{
    private readonly IRepository<Entities.FixedProduct.FixedProductColor> _colorRepo;

    public AddFixedProductSizeValidator(IRepository<Entities.FixedProduct.FixedProductColor> colorRepo)
    {
        _colorRepo = colorRepo;

        RuleFor(x => x.ColorId).GreaterThan(0)
            .MustAsync(async (id, cancellation) =>
            {
                return await _colorRepo.Get().AnyAsync(c => c.Id == id, cancellation);
            })
            .WithMessage("Product color not found.");

        RuleFor(x => x.Quantity)
            .GreaterThanOrEqualTo(0).WithMessage("Quantity cannot be negative.");

        RuleFor(x => x.Size)
            .IsInEnum().WithMessage("Invalid size. Please select a valid size.")
            .MustAsync(async (dto, sizeEnum, cancellation) =>
            {
                var currentColor = await _colorRepo.Get()
                    .Include(c => c.Sizes) 
                    .FirstOrDefaultAsync(c => c.Id == dto.ColorId, cancellation);

                if (currentColor == null) return true;

                bool sizeExists = currentColor.Sizes.Any(s => s.Size == sizeEnum);

                return !sizeExists; 
            })
            .WithMessage("This size already exists for the selected color. Please update the quantity instead.");
    }
}