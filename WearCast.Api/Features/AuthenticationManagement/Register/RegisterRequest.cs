namespace WearCast.Api.Features.AuthenticationManagement.Register
{
    public record RegisterRequest(
        string Email,
        string Password,
        string FirstName,
        string LastName,
        string PhoneNumber
        ): IRequest<Result<RegisterResponse>>;
}
