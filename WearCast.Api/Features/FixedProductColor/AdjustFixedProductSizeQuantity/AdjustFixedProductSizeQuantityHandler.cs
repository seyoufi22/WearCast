using WearCast.Api.Features.FixedProductSize.AdjustFixedProductSizeQuantity.DTOs;

namespace WearCast.Api.Features.FixedProductSize.AdjustFixedProductSizeQuantity;

public class AdjustFixedProductSizeQuantityHandler : IRequestHandler<AdjustFixedProductSizeQuantityRequestDto, bool>
{
    private readonly IRepository<Entities.FixedProduct.FixedProductColor> _colorRepo;

    public AdjustFixedProductSizeQuantityHandler(IRepository<Entities.FixedProduct.FixedProductColor> colorRepo)
    {
        _colorRepo = colorRepo;
    }

    public async Task<bool> Handle(AdjustFixedProductSizeQuantityRequestDto request, CancellationToken cancellationToken)
    {
        var color = await _colorRepo.Get()
            .Include(c => c.Sizes)
            .FirstOrDefaultAsync(c => c.Id == request.ColorId, cancellationToken);

        if (color == null)
            return false;

        var existingSize = color.Sizes.FirstOrDefault(s => s.Size == request.Size);

        if (existingSize == null)
            return false;

        existingSize.Quantity += request.Quantity;

        await _colorRepo.UpdateAsync(color);

        return true;
    }
}