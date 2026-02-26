using WearCast.Api.Features.Drivers.CreateDriver;

namespace WearCast.Api.Features.Drivers
{
    public class DriverProfile : Profile
    {
        public DriverProfile()
        {
            CreateMap<CreateDriverRequest, ApplicationUser>()
                .ForMember(dest => dest.UserName, opt => opt.MapFrom(src => src.Email))

                .ForPath(dest => dest.Address.State, opt => opt.MapFrom(src => src.State))
                .ForPath(dest => dest.Address.City, opt => opt.MapFrom(src => src.City))
                .ForPath(dest => dest.Address.Street, opt => opt.MapFrom(src => src.Street))
                .ForPath(dest => dest.Address.BuildingNumber, opt => opt.MapFrom(src => src.BuildingNumber));

            CreateMap<CreateDriverRequest, Driver>()
                .ForMember(dest => dest.ProfileImageUrl, opt => opt.Ignore())

                .ForMember(dest => dest.Status, opt => opt.Ignore());
        }


    }
}
