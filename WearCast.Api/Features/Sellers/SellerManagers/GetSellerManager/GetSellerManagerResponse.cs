namespace WearCast.Api.Features.Sellers.SellerManagers.GetSellerManager;

public record GetSellerManagerResponse(
    int Id,
    string FirstName,
    string LastName,
    string? PhoneNumber
);