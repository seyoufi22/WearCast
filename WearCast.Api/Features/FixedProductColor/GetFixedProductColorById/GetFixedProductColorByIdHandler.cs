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
        var query = _colorRepo.Get()
            .Where(c => c.Id == request.ColorId)
            .Include(c => c.Sizes)
            .Include(c => c.Images)
            .AsNoTracking();

        var colorEntity = await query.FirstOrDefaultAsync(cancellationToken);
        
        if (colorEntity == null)
            return null;

        return _mapper.Map<GetFixedProductColorByIdResponseDto>(colorEntity);
    }
}
