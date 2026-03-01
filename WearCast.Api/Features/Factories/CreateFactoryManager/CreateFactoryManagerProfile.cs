namespace WearCast.Api.Features.Factories.CreateFactoryManager
{
    public class CreateFactoryManagerProfile : Profile
    {
        public CreateFactoryManagerProfile()
        {
            CreateMap<CreateFactoryManagerRequest, ApplicationUser>()
                .ForMember(dest => dest.UserName, opt => opt.MapFrom(src => src.Email))
                .ForMember(dest => dest.Id, opt => opt.Ignore())
                .ForMember(dest => dest.PasswordHash, opt => opt.Ignore());
        }
    }
}
