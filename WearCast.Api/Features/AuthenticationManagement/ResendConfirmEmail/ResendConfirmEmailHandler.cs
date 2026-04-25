using System.Security.Cryptography;

namespace WearCast.Api.Features.AuthenticationManagement.ResendConfirmEmail
{
    public class ResendConfirmEmailHandler(
        UserManager<ApplicationUser> userManager,
        ILogger<ResendConfirmEmailHandler> logger,
        EmailHelper emailHelper
        ) : IRequestHandler<ResendConfirmEmailRequest, Result<ResendConfirmEmailResponse>>
    {
        private readonly UserManager<ApplicationUser> _userManager = userManager;
        private readonly ILogger<ResendConfirmEmailHandler> _logger = logger;
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

            var updateResult = await _userManager.UpdateAsync(user);
            if (!updateResult.Succeeded)
            {
                _logger.LogError("Failed to update user {UserId} with new confirmation code.", user.Id);
                return Result.Failure<ResendConfirmEmailResponse>(new("User.UpdateFailed", "Failed to process the request.", 500));
            }

            _logger.LogInformation("Resent Confirmation OTP: {code} for user {Email}", code, user.Email);

            var roles = await _userManager.GetRolesAsync(user);
            var primaryRole = roles.FirstOrDefault();

            switch (primaryRole)
            {
                case DefaultRoles.Driver:
                    await _emailHelper.SendDriverConfirmationEmail(user, code);
                    break;
                case DefaultRoles.FactoryManager:
                    await _emailHelper.SendFactoryManagerConfirmationEmail(user, code);
                    break;
                case DefaultRoles.ShippingCompanyManager:
                    await _emailHelper.SendShippingCompanyManagerConfirmationEmail(user, code);
                    break;
                case DefaultRoles.Customer:
                    await _emailHelper.SendCustomerConfirmationEmail(user, code);
                    break;
            }

            return Result.Success(new ResendConfirmEmailResponse(user.Id));
        }
    }
}