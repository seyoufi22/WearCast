using WearCast.Api.Features.FixedProductColor.AddFixedProductSize.DTOs;

namespace WearCast.Api.Features.FixedProductSize.AddFixedProductSize.DTOs;

public record AddFixedProductSizeCommandDto(AddFixedProductSizeRequestDto request, bool Admin, int? sellerId) : IRequest<Result>;

public class AddFixedProductSizeValidator : AbstractValidator<AddFixedProductSizeCommandDto>
{
    public AddFixedProductSizeValidator()
    {

        RuleFor(x => x.request.ColorId).GreaterThan(0);

        RuleFor(x => x.request.Quantity)
            .GreaterThanOrEqualTo(0).WithMessage("Quantity cannot be negative.");

        RuleFor(x => x.request.Size)
            .IsInEnum().WithMessage("Invalid size. Please select a valid size.");
    }
}