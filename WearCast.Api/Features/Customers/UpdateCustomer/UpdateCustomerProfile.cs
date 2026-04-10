namespace WearCast.Api.Features.Customers.UpdateCustomer
{
    public class UpdateCustomerProfile : Profile
    {
        public UpdateCustomerProfile()
        {
            CreateMap<AddressDto, Address>();

            CreateMap<UpdateCustomerRequest, Customer>()
                .ForMember(dest => dest.Address, opt => opt.MapFrom(src => src.Address))

                .ForPath(dest => dest.ApplicationUser!.FirstName, opt => opt.MapFrom(src => src.FirstName))
                .ForPath(dest => dest.ApplicationUser!.LastName, opt => opt.MapFrom(src => src.LastName))
                .ForPath(dest => dest.ApplicationUser!.PhoneNumber, opt => opt.MapFrom(src => src.PhoneNumber));
        }
    }
}
