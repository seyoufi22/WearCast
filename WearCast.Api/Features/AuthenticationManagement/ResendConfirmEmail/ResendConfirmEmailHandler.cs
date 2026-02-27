
using System.Security.Cryptography;

namespace WearCast.Api.Features.AuthenticationManagement.ResendConfirmEmail
{
    public class ResendConfirmEmailHandler(
        UserManager<ApplicationUser> userManager,
        ILogger<ResendConfirmEmailHandler> _logger,
        EmailHelper emailHelper
        ) : IRequestHandler<ResendConfirmEmailRequest, Result<ResendSellerConfirmEmailResponse>>
    {
        private readonly UserManager<ApplicationUser> _userManager = userManager;
        private readonly ILogger<ResendConfirmEmailHandler> _logger = _logger;
        private readonly EmailHelper _emailHelper = emailHelper;

        public async Task<Result<ResendSellerConfirmEmailResponse>> Handle(ResendConfirmEmailRequest request, CancellationToken cancellationToken)
        {
            if (await _userManager.FindByEmailAsync(request.Email) is not { } user)
                return Result.Success(new ResendSellerConfirmEmailResponse(null!));

            if (user.EmailConfirmed)
                return Result.Failure<ResendSellerConfirmEmailResponse>(UserErrors.DublicatedConfirmation);

            var code = RandomNumberGenerator.GetInt32(100000, 1000000).ToString();
            user.EmailConfirmationCode = code;
            user.EmailConfirmationCodeExpiration = DateTime.UtcNow.AddMinutes(60);
            await _userManager.UpdateAsync(user);

            _logger.LogInformation("Confirmation OTP: {code}", code);

            await _emailHelper.SendConfirmationEmail(user, code);

            return Result.Success(new ResendSellerConfirmEmailResponse(user.Id));
        }
    }
}
