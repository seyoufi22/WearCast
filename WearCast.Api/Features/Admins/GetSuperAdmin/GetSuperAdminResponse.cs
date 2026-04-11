namespace WearCast.Api.Features.Admins.GetSuperAdmin;

public record GetSuperAdminResponse(
    string Id,
    string FirstName,
    string LastName,
    string? PhoneNumber
);