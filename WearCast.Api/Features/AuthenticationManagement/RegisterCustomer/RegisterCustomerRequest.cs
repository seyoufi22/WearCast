namespace WearCast.Api.Features.AuthenticationManagement.Register
{
    public record RegisterCustomerRequest(
        string Email,
        string Password,
        string FirstName,
        string LastName,
        string PhoneNumber
        ): IRequest<Result<RegisterCustomerResponse>>;
}
