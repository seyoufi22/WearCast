namespace WearCast.Api.Features.ShippingCompanies.ShippingCompanyManagers.GetShippingCompanyManager;

public record GetShippingCompanyManagerResponse(
    int Id,
    string FirstName,
    string LastName,
    string? PhoneNumber,
    string? Email
);