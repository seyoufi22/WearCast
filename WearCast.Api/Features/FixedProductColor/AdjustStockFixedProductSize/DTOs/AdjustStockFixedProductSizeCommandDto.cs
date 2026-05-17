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

        RuleForEach(x => x.request.Adjustments).ChildRules(adjustment =>
        {
            adjustment.RuleFor(a => a.Size).IsInEnum().WithMessage("Invalid size.");
        });
    }
}