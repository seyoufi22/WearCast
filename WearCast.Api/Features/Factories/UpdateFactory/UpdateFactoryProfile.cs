namespace WearCast.Api.Features.Factories.UpdateFactory
{
    public class UpdateFactoryProfile : Profile
    {
        public UpdateFactoryProfile()
        {
            CreateMap<UpdateFactoryRequest, Factory>()
                .ForMember(dest => dest.Address, opt => opt.MapFrom(src => src.Address))

                .ForMember(dest => dest.Id, opt => opt.Ignore())
                .ForMember(dest => dest.LogoUrl, opt => opt.Ignore())
                .ForMember(dest => dest.Managers, opt => opt.Ignore())
                .ForMember(dest => dest.DesignedProducts, opt => opt.Ignore())
                .ForMember(dest => dest.IsDeleted, opt => opt.Ignore());
        }
    }
}
