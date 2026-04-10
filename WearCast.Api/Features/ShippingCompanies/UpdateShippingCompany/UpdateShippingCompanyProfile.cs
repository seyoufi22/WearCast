namespace WearCast.Api.Features.ShippingCompanies.UpdateShippingCompany
{
    public class UpdateShippingCompanyProfile : Profile
    {
        public UpdateShippingCompanyProfile()
        {
            CreateMap<UpdateShippingCompanyRequest, ShippingCompany>()
                .ForMember(dest => dest.Address, opt => opt.MapFrom(src => src.Address))

                .ForMember(dest => dest.Id, opt => opt.Ignore())
                .ForMember(dest => dest.LogoUrl, opt => opt.Ignore())
                .ForMember(dest => dest.Managers, opt => opt.Ignore())
                .ForMember(dest => dest.Drivers, opt => opt.Ignore())
                .ForMember(dest => dest.Shipments, opt => opt.Ignore())
                .ForMember(dest => dest.IsDeleted, opt => opt.Ignore());
        }
    }
}
