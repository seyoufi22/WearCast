using WearCast.Api.Features.FixedProductColor.AdjustStockFixedProductSize.DTOs;
using WearCast.Api.Features.FixedProductColor.Errors;


namespace WearCast.Api.Features.FixedProductSize.AdjustStockFixedProductSize;

public class AdjustStockFixedProductSizeHandler : IRequestHandler<AdjustStockFixedProductSizeCommandDto, Result>
{
    private readonly IRepository<Entities.FixedProduct.FixedProductColor> _colorRepo;

    public AdjustStockFixedProductSizeHandler(IRepository<Entities.FixedProduct.FixedProductColor> colorRepo)
    {
        _colorRepo = colorRepo;
    }

    public async Task<Result> Handle(AdjustStockFixedProductSizeCommandDto command, CancellationToken cancellationToken)
    {
        var color = await _colorRepo.Get()
            .Include(c => c.Sizes)
            .Include(c => c.Product)
            .FirstOrDefaultAsync(c => c.Id == command.request.ColorId, cancellationToken);

        if (color is null)
            return Result.Failure(FixedProductColorErrors.ColorNotFound);

        if (!command.isAdminRequest && color.Product.SellerId != command.sellerId)
            return Result.Failure(AuthErrors.Forbidden);

        foreach (var adjustment in command.request.Adjustments)
        {
            var existingSize = color.Sizes.FirstOrDefault(s => s.Size == adjustment.Size);
            int currentQuantity = existingSize?.Quantity ?? 0;

            if (currentQuantity + adjustment.Quantity < 0)
                return Result.Failure(FixedProductColorErrors.InsufficientStock);
        }

        foreach (var adjustment in command.request.Adjustments)
        {
            color.AdjustSize(adjustment.Size, adjustment.Quantity);
        }

        await _colorRepo.UpdateAsync(color);

        return Result.Success();
    }
}