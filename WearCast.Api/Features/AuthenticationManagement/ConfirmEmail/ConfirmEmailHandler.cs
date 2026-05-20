using WearCast.Api.Features.Sellers; // مسار الـ Errors

namespace WearCast.Api.Features.AuthenticationManagement.ConfirmEmail
{
    public class ConfirmEmailHandler(
        UserManager<ApplicationUser> userManager,
        ApplicationDbContext context
        ) : IRequestHandler<ConfirmEmailRequest, Result>
    {
        private readonly UserManager<ApplicationUser> _userManager = userManager;
        private readonly ApplicationDbContext _context = context;

        public async Task<Result> Handle(ConfirmEmailRequest request, CancellationToken cancellationToken)
        {
            var user = await _userManager.FindByEmailAsync(request.Email);
            if (user != null)
            {
                if (user.EmailConfirmed)
                    return Result.Failure(UserErrors.DublicatedConfirmation);

                if (user.EmailConfirmationCode != request.Code || user.EmailConfirmationCodeExpiration < DateTime.UtcNow)
                    return Result.Failure(UserErrors.InvalidCode);

                user.EmailConfirmed = true;
                user.EmailConfirmationCode = null;
                user.EmailConfirmationCodeExpiration = null;
                await _userManager.UpdateAsync(user);

                return Result.Success();
            }

            var application = await _context.SellerApplications
                .FirstOrDefaultAsync(x => x.ManagerEmail == request.Email, cancellationToken);

            if (application != null)
            {
                if (application.ManagerEmailConfirmed)
                    return Result.Failure(SellerApplicationErrors.DublicatedConfirmation);

                if (application.ManagerEmailConfirmationCode != request.Code || application.ManagerEmailConfirmationCodeExpiration < DateTime.UtcNow)
                    return Result.Failure(SellerApplicationErrors.InvalidCode);

                application.ManagerEmailConfirmed = true;
                application.ManagerEmailConfirmationCode = null;
                application.ManagerEmailConfirmationCodeExpiration = null;

                _context.SellerApplications.Update(application);
                await _context.SaveChangesAsync(cancellationToken);

                return Result.Success();
            }

            return Result.Failure(UserErrors.InvalidCode);
        }
    }
}