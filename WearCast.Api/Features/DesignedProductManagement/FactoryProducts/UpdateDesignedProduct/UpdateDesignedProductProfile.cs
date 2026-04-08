namespace WearCast.Api.Features.DesignedProductManagement.FactoryProducts.UpdateDesignedProduct
{
    public class UpdateDesignedProductProfile : Profile
    {
        public UpdateDesignedProductProfile()
        {
            CreateMap<UpdateDesignedProductRequest, DesignedProduct>()
                .ForMember(dest => dest.Id, opt => opt.Ignore())
                .ForMember(dest => dest.FactoryId, opt => opt.Ignore())
                .ForMember(dest => dest.TargetAudience, opt => opt.Ignore());
        }
    }
}
