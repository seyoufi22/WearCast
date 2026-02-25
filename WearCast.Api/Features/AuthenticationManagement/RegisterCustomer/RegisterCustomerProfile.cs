namespace WearCast.Api.Features.AuthenticationManagement.Register
{
    public class RegisterCustomerProfile : Profile
    {
        public RegisterCustomerProfile()
        {
            CreateMap<RegisterCustomerRequest, ApplicationUser>()
                .ForMember(dest => dest.UserName, opt => opt.MapFrom(src => src.Email))

                .ForMember(dest => dest.Address, opt => opt.MapFrom(src => new Address
                {
                    State = src.State,
                    City = src.City,
                    Street = src.Street,
                    BuildingNumber = src.BuildingNumber
                }));
        }
    }
}
