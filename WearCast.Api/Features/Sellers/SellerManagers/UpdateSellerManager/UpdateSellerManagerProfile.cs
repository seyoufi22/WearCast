namespace WearCast.Api.Features.Sellers.SellerManagers.UpdateSellerManager
{
    public class UpdateSellerManagerProfile : Profile
    {
        public UpdateSellerManagerProfile()
        {
            CreateMap<UpdateSellerManagerRequest, ApplicationUser>()
                .ForMember(dest => dest.FirstName, opt => opt.MapFrom(src => src.FirstName))
                .ForMember(dest => dest.LastName, opt => opt.MapFrom(src => src.LastName))
                .ForMember(dest => dest.PhoneNumber, opt => opt.MapFrom(src => src.PhoneNumber));
        }
    }
}
