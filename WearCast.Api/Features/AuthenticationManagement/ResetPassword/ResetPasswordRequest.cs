namespace WearCast.Api.Features.AuthenticationManagement.ResetPassword
{
    public record ResetPasswordRequest(
        string Email,
        string Code,
        string NewPassword,
        string ConfirmNewPassword
        ) : IRequest<Result>;
}
