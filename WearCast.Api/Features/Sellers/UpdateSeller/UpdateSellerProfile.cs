namespace WearCast.Api.Features.Sellers.UpdateSeller
{
    public class UpdateSellerProfile : Profile
    {
        public UpdateSellerProfile()
        {
            CreateMap<UpdateSellerRequest, Seller>()
                .ForMember(dest => dest.Address, opt => opt.MapFrom(src => src.Address))

                .ForMember(dest => dest.Id, opt => opt.Ignore())
                .ForMember(dest => dest.LogoUrl, opt => opt.Ignore())
                .ForMember(dest => dest.Managers, opt => opt.Ignore())
                .ForMember(dest => dest.FixedProducts, opt => opt.Ignore())
                .ForMember(dest => dest.IsDeleted, opt => opt.Ignore());
        }
    }
}
