namespace WearCast.Api.Features.Sellers.SellerManagers.CreateSellerManager
{
    public class CreateSellerManagerProfile : Profile
    {
        public CreateSellerManagerProfile()
        {
            CreateMap<CreateSellerManagerRequest, ApplicationUser>()
                .ForMember(dest => dest.UserName, opt => opt.MapFrom(src => src.Email))

                .ForMember(dest => dest.Id, opt => opt.Ignore())

                .ForMember(dest => dest.PasswordHash, opt => opt.Ignore());
        }
    }
}
