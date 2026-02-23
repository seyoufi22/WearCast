namespace WearCast.Api.Features.AuthenticationManagement.Register
{
    public record RegisterCustomerRequest(
        string Email,
        string Password,
        string ConfirmPassword,
        string FirstName,
        string LastName,
        string PhoneNumber,
        IFormFile? ProfileImage,
        string State,
        string City,
        string Street,
        string BuildingNumber
        ) : IRequest<Result<RegisterCustomerResponse>>;
}
