namespace WearCast.Api.Features.Sellers.GetSeller;

public record GetSellerResponse(
        string Name,
        string Email,
        string PhoneNumber,
        string CommercialRegisterNumber,
        string TaxIdNumber,
        string Description,
        string? logoUrl,
        AddressDto? Address);
