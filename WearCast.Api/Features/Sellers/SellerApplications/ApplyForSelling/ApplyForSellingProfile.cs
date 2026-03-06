namespace WearCast.Api.Features.Sellers.SellerApplications.ApplyForSelling
{
    public class ApplyForSellingProfile : Profile
    {
        public ApplyForSellingProfile()
        {
            CreateMap<ApplyForSellingRequest, SellerApplication>()
                .ForMember(dest => dest.ManagerFirstName, opt => opt.MapFrom(src => src.SellerManagerFirstName))
                .ForMember(dest => dest.ManagerLastName, opt => opt.MapFrom(src => src.SellerManagerLastName))
                .ForMember(dest => dest.ManagerEmail, opt => opt.MapFrom(src => src.SellerManagerEmail))
                .ForMember(dest => dest.ManagerPhoneNumber, opt => opt.MapFrom(src => src.SellerManagerPhoneNumber))


                .ForMember(dest => dest.CommercialRegisterNumber, opt => opt.MapFrom(src => src.SellerCommercialRegisterNumber))
                .ForMember(dest => dest.TaxIdNumber, opt => opt.MapFrom(src => src.SellerTaxIdNumber))
                .ForMember(dest => dest.Description, opt => opt.MapFrom(src => src.SellerDescription))

                .ForPath(dest => dest.SellerAddress.State, opt => opt.MapFrom(src => src.SellerState))
                .ForPath(dest => dest.SellerAddress.City, opt => opt.MapFrom(src => src.SellerCity))
                .ForPath(dest => dest.SellerAddress.Street, opt => opt.MapFrom(src => src.SellerStreet))
                .ForPath(dest => dest.SellerAddress.BuildingNumber, opt => opt.MapFrom(src => src.SellerBuildingNumber))

                .ForMember(dest => dest.LogoUrl, opt => opt.Ignore())
                .ForMember(dest => dest.ManagerPasswordHash, opt => opt.Ignore())
                .ForMember(dest => dest.ManagerEmailConfirmationCode, opt => opt.Ignore())
                .ForMember(dest => dest.ManagerEmailConfirmationCodeExpiration, opt => opt.Ignore())
                .ForMember(dest => dest.ManagerEmailConfirmed, opt => opt.Ignore())
                .ForMember(dest => dest.Id, opt => opt.Ignore())
                .ForMember(dest => dest.Status, opt => opt.Ignore())
                .ForMember(dest => dest.CreatedOn, opt => opt.Ignore())
                .ForMember(dest => dest.RejectionReason, opt => opt.Ignore());
        }
    }
}
