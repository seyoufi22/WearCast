namespace WearCast.Api.Features.AccountManagement.ChangePassword
{
    public record ChangePasswordRequest(
        string CurrentPassword,
        string NewPassword
        ) : IRequest<Result>;
}
