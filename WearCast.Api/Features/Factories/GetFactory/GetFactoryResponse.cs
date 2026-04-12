namespace WearCast.Api.Features.Factories.GetFactory;

public record GetFactoryResponse(
    int Id,
    string Name,
    string Email,
    string PhoneNumber,
    string CommercialRegisterNumber,
    string TaxIdNumber,
    string Description,
    string? LogoUrl,
    AddressDto? Address
);