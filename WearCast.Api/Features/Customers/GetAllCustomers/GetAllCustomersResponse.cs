namespace WearCast.Api.Features.Customers.GetAllCustomers;

public record GetAllCustomersResponse(
    int Id,
    string FirstName,
    string LastName,
    string Email,
    string PhoneNumber,
    string? ImageUrl,
    string? City
);