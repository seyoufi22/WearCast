namespace WearCast.Api.Features.ShippingCompanies.CreateShippingCompany
{
    public class CreateShippingCompanyProfile : Profile
    {
        public CreateShippingCompanyProfile()
        {
            CreateMap<CreateShippingCompanyRequest, ShippingCompany>()
                .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.CompanyName))
                .ForMember(dest => dest.Email, opt => opt.MapFrom(src => src.CompanyEmail))
                .ForMember(dest => dest.PhoneNumber, opt => opt.MapFrom(src => src.CompanyPhoneNumber))

                .ForPath(dest => dest.Address.State, opt => opt.MapFrom(src => src.CompanyState))
                .ForPath(dest => dest.Address.City, opt => opt.MapFrom(src => src.CompanyCity))
                .ForPath(dest => dest.Address.Street, opt => opt.MapFrom(src => src.CompanyStreet))
                .ForPath(dest => dest.Address.BuildingNumber, opt => opt.MapFrom(src => src.CompanyBuildingNumber))

                .ForMember(dest => dest.Id, opt => opt.Ignore())
                .ForMember(dest => dest.LogoUrl, opt => opt.Ignore())
                .ForMember(dest => dest.Managers, opt => opt.Ignore());

            CreateMap<CreateShippingCompanyRequest, ApplicationUser>()
                .ForMember(dest => dest.UserName, opt => opt.MapFrom(src => src.ManagerEmail))
                .ForMember(dest => dest.Email, opt => opt.MapFrom(src => src.ManagerEmail))
                .ForMember(dest => dest.FirstName, opt => opt.MapFrom(src => src.ManagerFirstName))
                .ForMember(dest => dest.LastName, opt => opt.MapFrom(src => src.ManagerLastName))
                .ForMember(dest => dest.PhoneNumber, opt => opt.MapFrom(src => src.ManagerPhoneNumber))

                .ForMember(dest => dest.Id, opt => opt.Ignore())
                .ForMember(dest => dest.PasswordHash, opt => opt.Ignore());
        }
    }
}
