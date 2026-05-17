namespace WearCast.Api.Features.Customers.GetCustomer;

public record GetCustomerResponse(
    int Id,
    string FirstName,
    string LastName,
    string PhoneNumber,
    string Email,
    string Imageurl,
    AddressDto Address
);