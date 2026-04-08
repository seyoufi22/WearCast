using WearCast.Api.Features.FixedProductSize.AdjustStockFixedProductSize.DTOs;

namespace WearCast.Api.Features.FixedProductColor.AdjustStockFixedProductSize.DTOs;

public record AdjustStockFixedProductSizeCommandDto
(
    AdjustStockFixedProductSizeRequestDto request, int sellerId, bool isAdminRequest
) : IRequest<Result>;
public class AdjustFixedProductSizeQuantityValidator : AbstractValidator<AdjustStockFixedProductSizeCommandDto>
{

    public AdjustFixedProductSizeQuantityValidator()
    {
        RuleFor(x => x.request.ColorId).GreaterThan(0);

        RuleFor(x => x.request.Size)
            .IsInEnum().WithMessage("Invalid size. Please select a valid size.");
    }
}