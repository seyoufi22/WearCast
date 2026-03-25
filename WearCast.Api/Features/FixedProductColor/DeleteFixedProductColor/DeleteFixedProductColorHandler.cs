using WearCast.Api.Features.FixedProductColor.DeleteFixedProductColor.DTOs;
using WearCast.Api.Features.FixedProductColor.Errors;

namespace WearCast.Api.Features.FixedProductColor.DeleteFixedProductColor;

public class DeleteFixedProductColorHandler : IRequestHandler<DeleteFixedProductColorRequestDto, Result>
{
    private readonly IRepository<Entities.FixedProduct.FixedProductColor> _colorRepository;

    public DeleteFixedProductColorHandler(IRepository<Entities.FixedProduct.FixedProductColor> colorRepository)
    {
        _colorRepository = colorRepository;
    }

    public async Task<Result> Handle(DeleteFixedProductColorRequestDto request, CancellationToken cancellationToken)
    {
        var color = await _colorRepository.Get().Include(c=>c.Product)
            .FirstOrDefaultAsync(c => c.Id == request.ColorId);

        if (color is null)
            return Result.Failure(FixedProductColorErrors.ColorNotFound);

        if(!request.isAdminRequest && color.Product.SellerId != request.sellerId)
            return Result.Failure(AuthErrors.Forbidden);

        await _colorRepository.SoftDeleteAsync(request.ColorId);

        return Result.Success();
    }
}