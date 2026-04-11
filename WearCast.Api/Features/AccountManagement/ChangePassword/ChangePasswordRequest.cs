namespace WearCast.Api.Features.AccountManagement.ChangePassword
{
    public record ChangePasswordRequest(
        string CurrentPassword,
        string NewPassword,
        string ConfirmNewPassword
        ) : IRequest<Result>;
}
