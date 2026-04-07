namespace WearCast.Api.Features.Customers.UpdateCustomer
{
    public record UpdateCustomerRequest(
        string FirstName,
        string LastName,
        string PhoneNumber,
        AddressDto Address,
        int? ProvidedCustomerId = null
        ) : IRequest<Result>;
}
