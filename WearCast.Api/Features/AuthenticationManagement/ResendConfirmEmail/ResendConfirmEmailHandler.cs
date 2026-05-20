using System.Security.Cryptography;
using WearCast.Api.Features.Sellers;

namespace WearCast.Api.Features.AuthenticationManagement.ResendConfirmEmail
{
    public class ResendConfirmEmailHandler(
        UserManager<ApplicationUser> userManager,
        ApplicationDbContext context,
        ILogger<ResendConfirmEmailHandler> logger,
        EmailHelper emailHelper
        ) : IRequestHandler<ResendConfirmEmailRequest, Result<ResendConfirmEmailResponse>>
    {
        private readonly UserManager<ApplicationUser> _userManager = userManager;
        private readonly ApplicationDbContext _context = context;
        private readonly ILogger<ResendConfirmEmailHandler> _logger = logger;
        private readonly EmailHelper _emailHelper = emailHelper;

        public async Task<Result<ResendConfirmEmailResponse>> Handle(ResendConfirmEmailRequest request, CancellationToken cancellationToken)
        {
            var code = RandomNumberGenerator.GetInt32(100000, 1000000).ToString();
            var expiration = DateTime.UtcNow.AddMinutes(60);

            var user = await _userManager.FindByEmailAsync(request.Email);
            if (user != null)
            {
                if (user.EmailConfirmed)
                    return Result.Failure<ResendConfirmEmailResponse>(UserErrors.DublicatedConfirmation);

                user.EmailConfirmationCode = code;
                user.EmailConfirmationCodeExpiration = expiration;

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
                    case DefaultRoles.SellerManager:
                        await _emailHelper.SendSellerManagerConfirmationEmail(user, code);
                        break;
                }

                return Result.Success(new ResendConfirmEmailResponse(user.Id));
            }

            var application = await _context.SellerApplications
                .FirstOrDefaultAsync(x => x.ManagerEmail == request.Email, cancellationToken);

            if (application != null)
            {
                if (application.ManagerEmailConfirmed)
                    return Result.Failure<ResendConfirmEmailResponse>(SellerApplicationErrors.DublicatedConfirmation);

                application.ManagerEmailConfirmationCode = code;
                application.ManagerEmailConfirmationCodeExpiration = expiration;

                _context.SellerApplications.Update(application);
                await _context.SaveChangesAsync(cancellationToken);

                await _emailHelper.SendConfirmationEmailForSellerManager(application, code);

                return Result.Success(new ResendConfirmEmailResponse(application.ManagerEmail));
            }
            return Result.Success(new ResendConfirmEmailResponse(null!));
        }
    }
}