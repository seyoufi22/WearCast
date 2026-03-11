namespace WearCast.Api.Features.DesignedProductManagement.FactoryProducts.CreateDesignedProduct
{
    public class CreateDesignedProductProfile : Profile
    {
        public CreateDesignedProductProfile()
        {
            CreateMap<CreateDesignedProductRequest, DesignedProduct>()
                 .ForMember(dest => dest.Slug, opt => opt.MapFrom(src => src.Name.ToUniqueSlug()))
                 .ForMember(dest => dest.FactoryId, opt => opt.Ignore());

            CreateMap<CreateProductSizeRequest, DesignedProductSizeDetails>();
        }
    }
}
