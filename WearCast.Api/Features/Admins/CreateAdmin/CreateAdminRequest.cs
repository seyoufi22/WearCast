namespace WearCast.Api.Features.Admins.CreateAdmin
{
    public record CreateAdminRequest(
        string Email,
        string FirstName,
        string LastName,
        string PhoneNumber,
        string Password,
        string ConfirmPassword,
        AdminRole Role
        ) : IRequest<Result<CreateAdminResponse>>;
}
