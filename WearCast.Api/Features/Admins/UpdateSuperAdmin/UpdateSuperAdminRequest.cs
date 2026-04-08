namespace WearCast.Api.Features.Admins.UpdateSuperAdmin
{
    public record UpdateSuperAdminRequest(
         string FirstName,
         string LastName,
         string PhoneNumber
        ) : IRequest<Result>;
}
