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
            .Include(c=>c.Product)
            .FirstOrDefaultAsync(c => c.Id == command.request.ColorId, cancellationToken);

        if (color is null)
            return Result.Failure(FixedProductColorErrors.ColorNotFound);

        if (!command.isAdminRequest && color.Product.SellerId != command.sellerId)
            return Result.Failure(AuthErrors.Forbidden);

        var existingSize = color.Sizes.FirstOrDefault(s => s.Size == command.request.Size);

        if (existingSize is null)
        {
            if (command.request.Quantity < 0)
                return Result.Failure(FixedProductColorErrors.InsufficientStock);
            else
                color.AdjustSize(command.request.Size, command.request.Quantity);
            await _colorRepo.UpdateAsync(color);
            return Result.Success();
        }

        if (existingSize.Quantity + command.request.Quantity < 0)
            return Result.Failure(FixedProductColorErrors.InsufficientStock);

        color.AdjustSize(command.request.Size, command.request.Quantity);

        await _colorRepo.UpdateAsync(color);

        return Result.Success();
    }
}