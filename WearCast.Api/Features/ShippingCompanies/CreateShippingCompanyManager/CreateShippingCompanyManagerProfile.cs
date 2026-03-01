namespace WearCast.Api.Features.ShippingCompanies.CreateShippingCompanyManager
{
    public class CreateShippingCompanyManagerProfile : Profile
    {
        public CreateShippingCompanyManagerProfile()
        {
            CreateMap<CreateShippingCompanyManagerRequest, ApplicationUser>()
                .ForMember(dest => dest.UserName, opt => opt.MapFrom(src => src.Email))
                .ForMember(dest => dest.Id, opt => opt.Ignore())
                .ForMember(dest => dest.PasswordHash, opt => opt.Ignore());
        }
    }
}
