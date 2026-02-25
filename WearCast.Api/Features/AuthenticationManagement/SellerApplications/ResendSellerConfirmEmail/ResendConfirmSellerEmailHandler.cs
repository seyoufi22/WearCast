
using System.Security.Cryptography;
using WearCast.Api.Common.Email;

namespace WearCast.Api.Features.AuthenticationManagement.SellerApplications.ResendSellerConfirmEmail
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
            var application = await _context.SellerApplications.FirstOrDefaultAsync(x => x.Email == request.Email, cancellationToken);

            if (application == null)
                return Result.Failure<ResendConfirmSellerEmailResponse>(SellerApplicationErrors.ApplicationNotFound);

            if (application.EmailConfirmed)
                return Result.Failure<ResendConfirmSellerEmailResponse>(SellerApplicationErrors.DublicatedConfirmation);

            var code = RandomNumberGenerator.GetInt32(100000, 1000000).ToString();
            application.EmailConfirmationCode = code;
            application.EmailConfirmationCodeExpiration = DateTime.UtcNow.AddMinutes(60);

            _context.SellerApplications.Update(application);
            await _context.SaveChangesAsync(cancellationToken);

            await _emailHelper.SendConfirmationEmailForSellerApplication(application, code);

            return Result.Success(new ResendConfirmSellerEmailResponse(application.Email));
        }
    }
}
