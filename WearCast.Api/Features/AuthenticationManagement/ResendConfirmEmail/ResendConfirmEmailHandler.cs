
using System.Security.Cryptography;
using WearCast.Api.Common.Email;

namespace WearCast.Api.Features.AuthenticationManagement.ResendConfirmEmail
{
    public class ResendConfirmEmailHandler(
        UserManager<ApplicationUser> userManager,
        ILogger<ResendConfirmEmailHandler> _logger,
        EmailHelper emailHelper
        ) : IRequestHandler<ResendConfirmEmailRequest, Result<ResendConfirmEmailResponse>>
    {
        private readonly UserManager<ApplicationUser> _userManager = userManager;
        private readonly ILogger<ResendConfirmEmailHandler> _logger = _logger;
        private readonly EmailHelper _emailHelper = emailHelper;

        public async Task<Result<ResendConfirmEmailResponse>> Handle(ResendConfirmEmailRequest request, CancellationToken cancellationToken)
        {
            if (await _userManager.FindByEmailAsync(request.Email) is not { } user)
                return Result.Success(new ResendConfirmEmailResponse(null!));

            if (user.EmailConfirmed)
                return Result.Failure<ResendConfirmEmailResponse>(UserErrors.DublicatedConfirmation);

            var code = RandomNumberGenerator.GetInt32(100000, 1000000).ToString();
            user.EmailConfirmationCode = code;
            user.EmailConfirmationCodeExpiration = DateTime.UtcNow.AddMinutes(60);
            await _userManager.UpdateAsync(user);

            _logger.LogInformation("Confirmation OTP: {code}", code);

            await _emailHelper.SendConfirmationEmail(user, code);

            return Result.Success(new ResendConfirmEmailResponse(user.Id));
        }
    }
}
