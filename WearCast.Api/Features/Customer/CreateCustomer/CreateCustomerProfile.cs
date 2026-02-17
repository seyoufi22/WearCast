using AutoMapper;
using WearCast.Api.Entities;

namespace WearCast.Api.Features.Customer.CreateCustomer;

public class CreateCustomerProfile : Profile
{
    public CreateCustomerProfile()
    {
        CreateMap<ApplicationUser, Entities.Customer>()
            .ForMember(dest => dest.Id, opt => opt.Ignore())
            .ForMember(dest => dest.Email, opt => opt.MapFrom(src => src.Email))
            .ForMember(dest => dest.PhoneNumber, opt => opt.MapFrom(src => src.PhoneNumber ?? string.Empty))
            .ForMember(dest => dest.ApplicationUserId, opt => opt.MapFrom(src => src.Id));
    }
}
