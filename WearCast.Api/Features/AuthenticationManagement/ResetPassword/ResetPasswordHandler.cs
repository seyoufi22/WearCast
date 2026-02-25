
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.WebUtilities;
using System.Text;
using WearCast.Api.Entities.Identity;

namespace WearCast.Api.Features.AuthenticationManagement.ResetPassword
{
    public class ResetPasswordHandler(
        UserManager<ApplicationUser> userManager
        ) : IRequestHandler<ResetPasswordRequest, Result>
    {
        private readonly UserManager<ApplicationUser> _userManager = userManager;

        public async Task<Result> Handle(ResetPasswordRequest request, CancellationToken cancellationToken)
        {
            var user = await _userManager.FindByEmailAsync(request.Email);

            if (user is null || !user.EmailConfirmed)
                return Result.Failure(UserErrors.InvalidCode);

            if (user.ResetPasswordCode != request.Code ||
                user.ResetPasswordCodeExpiration < DateTime.UtcNow)
            {
                return Result.Failure(UserErrors.InvalidCode);
            }

            var token = await _userManager.GeneratePasswordResetTokenAsync(user);

            var result = await _userManager.ResetPasswordAsync(user, token, request.NewPassword);

            if (!result.Succeeded)
            {
                var error = result.Errors.First();
                return Result.Failure(new Error(error.Code, error.Description, StatusCodes.Status401Unauthorized));
            }

            user.ResetPasswordCode = null;
            user.ResetPasswordCodeExpiration = null;

            await _userManager.UpdateAsync(user);

            return Result.Success();
        }
    }
}
