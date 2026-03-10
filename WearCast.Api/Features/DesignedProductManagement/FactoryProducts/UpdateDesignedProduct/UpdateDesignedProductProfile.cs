namespace WearCast.Api.Features.DesignedProductManagement.FactoryProducts.UpdateDesignedProduct
{
    public class UpdateDesignedProductProfile : Profile
    {
        public UpdateDesignedProductProfile()
        {
            CreateMap<UpdateDesignedProductRequest, DesignedProduct>()
                .ForMember(dest => dest.Id, opt => opt.Ignore())
                .ForMember(dest => dest.Slug, opt => opt.Ignore());
        }
    }
}
