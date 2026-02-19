using WearCast.Api.Entities.Identity;

namespace WearCast.Api.Features.AuthenticationManagement.Register
{
    public class RegisterProfile : Profile
    {
        public RegisterProfile() 
        {
            CreateMap<RegisterRequest, ApplicationUser>()
                .ForMember(dest => dest.UserName, opt => opt.MapFrom(src => src.Email));
        }
    }
}
