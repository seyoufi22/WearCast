namespace WearCast.Api.Features.Admins.CreateSuperAdmin
{
    public record CreateSuperAdminRequest(
        string Email,
        string FirstName,
        string LastName,
        string PhoneNumber,
        string Password,
        string ConfirmPassword
        ) : IRequest<Result>;
}
