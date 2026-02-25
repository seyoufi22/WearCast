

namespace WearCast.Api.Features.AuthenticationManagement.ConfirmEmail
{
    public class ConfirmEmailHandler(
        UserManager<ApplicationUser> userManager
        ) : IRequestHandler<ConfirmEmailRequest, Result>
    {
        private readonly UserManager<ApplicationUser> _userManager = userManager;

        public async Task<Result> Handle(ConfirmEmailRequest request, CancellationToken cancellationToken)
        {
            if (await _userManager.FindByIdAsync(request.UserId) is not { } user)
                return Result.Failure(UserErrors.InvalidCode);

            if (user.EmailConfirmed)
                return Result.Failure(UserErrors.DublicatedConfirmation);

            if (user.EmailConfirmationCode != request.Code ||
            user.EmailConfirmationCodeExpiration < DateTime.UtcNow)
            {
                return Result.Failure(UserErrors.InvalidCode);
            }

            user.EmailConfirmed = true;
            user.EmailConfirmationCode = null;
            user.EmailConfirmationCodeExpiration = null;
            await _userManager.UpdateAsync(user);

            return Result.Success();

        }
    }
}
