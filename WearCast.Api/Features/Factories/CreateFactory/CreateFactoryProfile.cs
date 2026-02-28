namespace WearCast.Api.Features.Factories.CreateFactory
{
    public class CreateFactoryProfile : Profile
    {
        public CreateFactoryProfile()
        {
            CreateMap<CreateFactoryRequest, Factory>()
                .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.FactoryName))
                .ForMember(dest => dest.Email, opt => opt.MapFrom(src => src.FactoryEmail))
                .ForMember(dest => dest.PhoneNumber, opt => opt.MapFrom(src => src.FactoryPhoneNumber))
                .ForMember(dest => dest.CommercialRegisterNumber, opt => opt.MapFrom(src => src.FactoryCommercialRegisterNumber))
                .ForMember(dest => dest.TaxIdNumber, opt => opt.MapFrom(src => src.FactoryTaxIdNumber))
                .ForMember(dest => dest.Description, opt => opt.MapFrom(src => src.FactoryDescription))

                .ForPath(dest => dest.Address.State, opt => opt.MapFrom(src => src.FactoryState))
                .ForPath(dest => dest.Address.City, opt => opt.MapFrom(src => src.FactoryCity))
                .ForPath(dest => dest.Address.Street, opt => opt.MapFrom(src => src.FactoryStreet))
                .ForPath(dest => dest.Address.BuildingNumber, opt => opt.MapFrom(src => src.FactoryBuildingNumber))

                .ForMember(dest => dest.Id, opt => opt.Ignore())
                .ForMember(dest => dest.LogoUrl, opt => opt.Ignore())
                .ForMember(dest => dest.Managers, opt => opt.Ignore());

            CreateMap<CreateFactoryRequest, ApplicationUser>()
                .ForMember(dest => dest.UserName, opt => opt.MapFrom(src => src.FactoryManagerEmail))
                .ForMember(dest => dest.Email, opt => opt.MapFrom(src => src.FactoryManagerEmail))
                .ForMember(dest => dest.FirstName, opt => opt.MapFrom(src => src.FactoryManagerFirstName))
                .ForMember(dest => dest.LastName, opt => opt.MapFrom(src => src.FactoryManagerLastName))
                .ForMember(dest => dest.PhoneNumber, opt => opt.MapFrom(src => src.FactoryManagerPhoneNumber))

                .ForMember(dest => dest.Id, opt => opt.Ignore())
                .ForMember(dest => dest.PasswordHash, opt => opt.Ignore());
        }
    }
}