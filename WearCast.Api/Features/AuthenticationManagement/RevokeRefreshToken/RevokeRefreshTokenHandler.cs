
using Microsoft.AspNetCore.Identity;
using Newtonsoft.Json.Linq;
using WearCast.Api.Entities;

namespace WearCast.Api.Features.AuthenticationManagement.RevokeRefreshToken
{
    public class RevokeRefreshTokenHandler(
        IJwtProvider jwtProvider,
        UserManager<ApplicationUser> userManager
        ) : IRequestHandler<RevokeRefreshTokenRequest, Result>
    {
        private readonly IJwtProvider _jwtProvider = jwtProvider;
        private readonly UserManager<ApplicationUser> _userManager = userManager;

        public async Task<Result> Handle(RevokeRefreshTokenRequest request, CancellationToken cancellationToken)
        {
            var userId = _jwtProvider.ValidateToken(request.Token);

            if (userId is null)
                return Result.Failure(UserErrors.InvalidJwtToken);

            var user = await _userManager.FindByIdAsync(userId);

            if (user is null)
                return Result.Failure(UserErrors.InvalidJwtToken);

            var userRefreshToken = user.RefreshTokens.SingleOrDefault(x => x.Token == request.RefreshToken && x.IsActive);

            if (userRefreshToken is null)
                return Result.Failure(UserErrors.InvalidRefreshToken);

            userRefreshToken.RevokedOn = DateTime.UtcNow;

            await _userManager.UpdateAsync(user);

            return Result.Success();
        }
    }
}
