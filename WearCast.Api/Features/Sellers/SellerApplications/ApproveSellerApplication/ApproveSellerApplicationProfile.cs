namespace WearCast.Api.Features.Sellers.SellerApplications.ApproveSellerApplication
{
    public class ApproveSellerApplicationProfile : Profile
    {
        public ApproveSellerApplicationProfile()
        {
            CreateMap<SellerApplication, ApplicationUser>()
                .ForMember(dest => dest.Id, opt => opt.Ignore())
                .ForMember(dest => dest.UserName, opt => opt.MapFrom(src => src.ManagerEmail))
                .ForMember(dest => dest.Email, opt => opt.MapFrom(src => src.ManagerEmail))
                .ForMember(dest => dest.PhoneNumber, opt => opt.MapFrom(src => src.ManagerPhoneNumber))
                .ForMember(dest => dest.FirstName, opt => opt.MapFrom(src => src.ManagerFirstName))
                .ForMember(dest => dest.LastName, opt => opt.MapFrom(src => src.ManagerLastName))
                .ForMember(dest => dest.PasswordHash, opt => opt.MapFrom(src => src.ManagerPasswordHash));

            CreateMap<SellerApplication, Seller>()
                .ForMember(dest => dest.Id, opt => opt.Ignore())
                .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.SellerName))
                .ForMember(dest => dest.Email, opt => opt.MapFrom(src => src.SellerEmail))
                .ForMember(dest => dest.PhoneNumber, opt => opt.MapFrom(src => src.SellerPhoneNumber))
                .ForMember(dest => dest.Address, opt => opt.MapFrom(src => src.SellerAddress))
                .ForMember(dest => dest.Managers, opt => opt.Ignore());
        }
    }
}
