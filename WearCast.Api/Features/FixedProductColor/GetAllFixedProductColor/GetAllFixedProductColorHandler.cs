using WearCast.Api.Features.FixedProductColor.GetAllFixedProductColor.DTOs;
namespace WearCast.Api.Features.FixedProductColor.GetAllFixedProductColor;

public class GetAllFixedProductColorHandler : IRequestHandler<GetAllFixedProductColorRequestDto, List<GetAllFixedProductColorResponseDto>>
{
    private readonly IRepository<Entities.FixedProduct.FixedProductColor> _colorRepo;

    public GetAllFixedProductColorHandler(IRepository<Entities.FixedProduct.FixedProductColor> colorRepo)
    {
        _colorRepo = colorRepo;
    }

    public async Task<List<GetAllFixedProductColorResponseDto>> Handle(GetAllFixedProductColorRequestDto request, CancellationToken cancellationToken)
    {
        // استخدام الميثود الجينيريك الجديدة مع فلتر الـ ProductId
        var colors = await _colorRepo.GetListAsync(c => c.ProductId == request.ProductId);

        return colors
            .Select(c => new GetAllFixedProductColorResponseDto(
                c.Id,
                c.ColorName,
                c.ColorCode))
            .ToList();
    }
}