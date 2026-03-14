
using System.Security.Cryptography;

namespace WearCast.Api.Features.Sellers.SellerApplications.ResendSellerConfirmEmail
{
    public class ResendConfirmSellerEmailHandler(
        ApplicationDbContext context,
        EmailHelper emailHelper
        ) : IRequestHandler<ResendConfirmSellerEmailRequest, Result<ResendConfirmSellerEmailResponse>>
    {
        private readonly ApplicationDbContext _context = context;
        private readonly EmailHelper _emailHelper = emailHelper;

        public async Task<Result<ResendConfirmSellerEmailResponse>> Handle(ResendConfirmSellerEmailRequest request, CancellationToken cancellationToken)
        {
            var application = await _context.SellerApplications.FirstOrDefaultAsync(x => x.ManagerEmail == request.Email, cancellationToken);

            if (application == null)
                return Result.Failure<ResendConfirmSellerEmailResponse>(SellerApplicationErrors.ApplicationNotFound);

            if (application.ManagerEmailConfirmed)
                return Result.Failure<ResendConfirmSellerEmailResponse>(SellerApplicationErrors.DublicatedConfirmation);

            var code = RandomNumberGenerator.GetInt32(100000, 1000000).ToString();
            application.ManagerEmailConfirmationCode = code;
            application.ManagerEmailConfirmationCodeExpiration = DateTime.UtcNow.AddMinutes(60);

            _context.SellerApplications.Update(application);
            await _context.SaveChangesAsync(cancellationToken);

            await _emailHelper.SendConfirmationEmailForSellerManager(application, code);

            return Result.Success(new ResendConfirmSellerEmailResponse(application.ManagerEmail));
        }
    }
}
