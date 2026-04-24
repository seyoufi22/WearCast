namespace WearCast.Api.Features.Admins.UpdateAdmin
{
    public record UpdateAdminRequest(
        string AdminId,
        string Email,
        string FirstName,
        string LastName,
        string PhoneNumber,
        AdminRole Role
        ) : IRequest<Result>;
}
