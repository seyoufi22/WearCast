namespace WearCast.Api.Features.Sellers.GetSeller;

public record GetSellerResponse(
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