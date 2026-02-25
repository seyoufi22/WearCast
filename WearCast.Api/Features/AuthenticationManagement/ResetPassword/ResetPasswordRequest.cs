namespace WearCast.Api.Features.AuthenticationManagement.ResetPassword
{
    public record ResetPasswordRequest(
        string Email,
        string Code,
        string NewPassword
        ) : IRequest<Result>;
}
