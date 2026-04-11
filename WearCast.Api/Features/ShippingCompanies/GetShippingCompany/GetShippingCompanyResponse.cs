namespace WearCast.Api.Features.ShippingCompanies.GetShippingCompany;

public record GetShippingCompanyResponse(
    int Id,
    string Name,
    string Email,
    string PhoneNumber,
    string CommercialRegisterNumber,
    string TaxIdNumber,
    string Description,
    decimal DeliveryFee,
    string? LogoUrl,
    AddressDto? Address
);