using WearCast.Api.Entities.Identity;

namespace WearCast.Api.Features.AuthenticationManagement.Register
{
    public class RegisterCustomerProfile : Profile
    {
        public RegisterCustomerProfile() 
        {
            CreateMap<RegisterCustomerRequest, ApplicationUser>()
                .ForMember(dest => dest.UserName, opt => opt.MapFrom(src => src.Email));
        }
    }
}
