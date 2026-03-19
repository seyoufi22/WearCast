using WearCast.Api.Features.FixedProductColor.Errors;
using WearCast.Api.Features.FixedProductSize.AddFixedProductSize.DTOs;

namespace WearCast.Api.Features.FixedProductSize.AddFixedProductSize;

public class AddFixedProductSizeHandler : IRequestHandler<AddFixedProductSizeCommandDto, Result>
{
    private readonly IRepository<Entities.FixedProduct.FixedProductColor> _colorRepo;

    public AddFixedProductSizeHandler(IRepository<Entities.FixedProduct.FixedProductColor> colorRepo)
    {
        _colorRepo = colorRepo;
    }

    public async Task<Result> Handle(AddFixedProductSizeCommandDto command, CancellationToken cancellationToken)
    {
        var color = await _colorRepo.Get()
            .Include(c => c.Sizes)
            .FirstOrDefaultAsync(c => c.Id == command.request.ColorId, cancellationToken);

        if (color == null)
            return Result.Failure(FixedProductColorErrors.ColorNotFound);

        if (color.Sizes.Any(s => s.Size == command.request.Size))
            return Result.Failure(FixedProductColorErrors.SizeAlreadyExists);

        if (!command.Admin && command.sellerId != color.Product.SellerId)
            return Result.Failure(AuthErrors.Forbidden);

        var newSize = new Entities.FixedProduct.FixedProductSize
            {
                Size = command.request.Size,
                Quantity = command.request.Quantity
            };

        color.AddSize(newSize);

        await _colorRepo.UpdateAsync(color);

        return Result.Success();
    }
}