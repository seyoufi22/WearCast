

namespace WearCast.Api.Features.AuthenticationManagement.Login
{
    public record LoginRequest(
        string Email,
        string Password
        ) : IRequest<Result<AuthResponse>>;
}
