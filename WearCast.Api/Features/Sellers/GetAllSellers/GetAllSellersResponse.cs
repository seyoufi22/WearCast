namespace WearCast.Api.Features.Sellers.GetAllSellers;

public record GetAllSellersResponse(
    int Id,
    string Name,
    string Email,
    string PhoneNumber,
    string? LogoUrl,
    string? City
);