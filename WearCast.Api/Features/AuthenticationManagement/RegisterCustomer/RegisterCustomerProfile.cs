namespace WearCast.Api.Features.AuthenticationManagement.Register
{
    public class RegisterCustomerProfile : Profile
    {
        public RegisterCustomerProfile()
        {
            CreateMap<RegisterCustomerRequest, ApplicationUser>()
                .ForMember(dest => dest.UserName, opt => opt.MapFrom(src => src.Email));

            CreateMap<RegisterCustomerRequest, Customer>()
                .ForPath(dest => dest.Address.State, opt => opt.MapFrom(src => src.State))
                .ForPath(dest => dest.Address.City, opt => opt.MapFrom(src => src.City))
                .ForPath(dest => dest.Address.Street, opt => opt.MapFrom(src => src.Street))
                .ForPath(dest => dest.Address.BuildingNumber, opt => opt.MapFrom(src => src.BuildingNumber));

        }
    }
}
