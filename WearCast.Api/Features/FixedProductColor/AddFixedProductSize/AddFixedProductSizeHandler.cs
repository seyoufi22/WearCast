using WearCast.Api.Features.FixedProductSize.AddFixedProductSize.DTOs;

namespace WearCast.Api.Features.FixedProductSize.AddFixedProductSize;

public class AddFixedProductSizeHandler : IRequestHandler<AddFixedProductSizeRequestDto, bool>
{
    private readonly IRepository<Entities.FixedProduct.FixedProductColor> _colorRepo;

    public AddFixedProductSizeHandler(IRepository<Entities.FixedProduct.FixedProductColor> colorRepo)
    {
        _colorRepo = colorRepo;
    }

    public async Task<bool> Handle(AddFixedProductSizeRequestDto request, CancellationToken cancellationToken)
    {
        var color = await _colorRepo.Get()
            .Include(c => c.Sizes)
            .FirstOrDefaultAsync(c => c.Id == request.ColorId, cancellationToken);

        if (color == null)
            return false;

        var newSize = new Entities.FixedProduct.FixedProductSize
        {
            Size = request.Size,
            Quantity = request.Quantity
        };

        color.AddSize(newSize);

        await _colorRepo.UpdateAsync(color);

        return true;
    }
}