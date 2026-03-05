using WearCast.Api.Features.FixedProductColor.GetFixedProductColorById.DTOs;

namespace WearCast.Api.Features.FixedProductColor.GetFixedProductColorById;

public class FixedProductColorProfile : Profile
{
    public FixedProductColorProfile()
    {
        CreateMap<Entities.FixedProduct.FixedProductSize, SizeDetailsDto>()
            .ForCtorParam("Size", opt => opt.MapFrom(src => src.Size.ToString()))
            .ForCtorParam("Quantity", opt => opt.MapFrom(src => src.Quantity));

        CreateMap<Entities.FixedProduct.FixedProductColor, GetFixedProductColorByIdResponseDto>()
            .ForCtorParam("Sizes", opt => opt.MapFrom(src =>
                src.Sizes ?? new List<Entities.FixedProduct.FixedProductSize>()))

            .ForCtorParam("AdditionalImages", opt => opt.MapFrom(src =>
                src.Images != null
                    ? src.Images.Select(i => new ImageDetailsDto(i.Id, i.ImageUrl)).ToList()
                    : new List<ImageDetailsDto>()));
    }
}