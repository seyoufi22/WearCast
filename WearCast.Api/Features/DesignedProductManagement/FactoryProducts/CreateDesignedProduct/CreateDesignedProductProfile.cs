namespace WearCast.Api.Features.DesignedProductManagement.FactoryProducts.CreateDesignedProduct
{
    public class CreateDesignedProductProfile : Profile
    {
        public CreateDesignedProductProfile()
        {
            CreateMap<CreateDesignedProductRequest, DesignedProduct>()
                 .ForMember(dest => dest.FactoryId, opt => opt.Ignore())
                 .ForMember(dest => dest.TargetAudience, opt => opt.Ignore());

            CreateMap<CreateProductSizeRequest, DesignedProductSizeDetails>();
        }
    }
}
