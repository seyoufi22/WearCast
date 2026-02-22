using WearCast.Api.Features.AuthenticationManagement.SellerApplications.ApplyForSelling;

namespace WearCast.Api.Features.AuthenticationManagement.SellerApplications
{
    public class SellerApplicationProfile : Profile
    {
        public SellerApplicationProfile()
        {
            CreateMap<ApplyForSellingRequest, SellerApplication>()

                .ForMember(dest => dest.Id, opt => opt.Ignore())
                .ForMember(dest => dest.Status, opt => opt.Ignore())
                .ForMember(dest => dest.RejectionReason, opt => opt.Ignore())
                .ForMember(dest => dest.PasswordHash, opt => opt.Ignore())
                .ForMember(dest => dest.LogoUrl, opt => opt.Ignore())

                .ForPath(dest => dest.StoreAddress.State, opt => opt.MapFrom(src => src.State))
                .ForPath(dest => dest.StoreAddress.City, opt => opt.MapFrom(src => src.City))
                .ForPath(dest => dest.StoreAddress.Street, opt => opt.MapFrom(src => src.Street))
                .ForPath(dest => dest.StoreAddress.BuildingNumber, opt => opt.MapFrom(src => src.BuildingNumber));

            CreateMap<SellerApplication, SellerApplicationResponse>()
                .ForMember(dest => dest.State, opt => opt.MapFrom(src => src.StoreAddress.State))
                .ForMember(dest => dest.City, opt => opt.MapFrom(src => src.StoreAddress.City))
                .ForMember(dest => dest.Street, opt => opt.MapFrom(src => src.StoreAddress.Street))
                .ForMember(dest => dest.BuildingNumber, opt => opt.MapFrom(src => src.StoreAddress.BuildingNumber));

            CreateMap<SellerApplication, ApplicationUser>()
                .ForMember(dest => dest.Id, opt => opt.Ignore())
                .ForMember(dest => dest.UserName, opt => opt.MapFrom(src => src.Email))
                .ForMember(dest => dest.Address, opt => opt.MapFrom(src => src.StoreAddress));


            CreateMap<SellerApplication, Seller>()
                .ForMember(dest => dest.Id, opt => opt.Ignore())
                .ForMember(dest => dest.UserId, opt => opt.Ignore())
                .ForMember(dest => dest.ApplicationUser, opt => opt.Ignore());

        }
    }
}
