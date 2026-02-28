using WearCast.Api.Features.FixedProductColor.GetFixedProductColorById.DTOs;

namespace WearCast.Api.Features.FixedProductColor.GetFixedProductColorById;
public class GetFixedProductColorByIdHandler : IRequestHandler<GetFixedProductColorByIdRequestDto, GetFixedProductColorByIdResponseDto>
{
    private readonly IRepository<Entities.FixedProduct.FixedProductColor> _colorRepo;
    private readonly IMapper _mapper;
    public GetFixedProductColorByIdHandler(IRepository<Entities.FixedProduct.FixedProductColor> colorRepo,IMapper mapper)
    {
        _colorRepo = colorRepo;
        _mapper = mapper;
    }

    public async Task<GetFixedProductColorByIdResponseDto> Handle(GetFixedProductColorByIdRequestDto request, CancellationToken cancellationToken)
    {
        var color = await _colorRepo.GetAsync(c => c.Id == request.ColorId,
            include: q => q.Include(c => c.Sizes)
                   .Include(c => c.Images));
        if (color == null)
            return null;
        return _mapper.Map<GetFixedProductColorByIdResponseDto>(color);
    }
}
