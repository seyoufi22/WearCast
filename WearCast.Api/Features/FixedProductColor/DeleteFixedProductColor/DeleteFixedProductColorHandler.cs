using WearCast.Api.Features.FixedProductColor.DeleteFixedProductColor.DTOs;

namespace WearCast.Api.Features.FixedProductColor.DeleteFixedProductColor;

public class DeleteFixedProductColorHandler : IRequestHandler<DeleteFixedProductColorRequestDto, bool>
{
    private readonly IRepository<Entities.FixedProduct.FixedProductColor> _colorRepository;

    public DeleteFixedProductColorHandler(IRepository<Entities.FixedProduct.FixedProductColor> colorRepository)
    {
        _colorRepository = colorRepository;
    }

    public async Task<bool> Handle(DeleteFixedProductColorRequestDto request, CancellationToken cancellationToken)
    {
        var color = await _colorRepository.GetAsync(c => c.Id == request.ColorId);

        if (color == null)
        {
            return false;
        }

        await _colorRepository.SoftDeleteAsync(request.ColorId);

        return true;
    }
}