namespace WearCast.Api.Features.AuthenticationManagement.SellerApplications.ConfirmSellerEmail
{
    public class ConfirmSellerEmailHandler(
        ApplicationDbContext context
        ) : IRequestHandler<ConfirmSellerEmailRequest, Result>
    {
        private readonly ApplicationDbContext _context = context;

        public async Task<Result> Handle(ConfirmSellerEmailRequest request, CancellationToken cancellationToken)
        {
            var application = await _context.SellerApplications.FirstOrDefaultAsync(x => x.Email == request.Email, cancellationToken);

            if (application == null)
                return Result.Failure(SellerApplicationErrors.ApplicationNotFound);

            if (application.EmailConfirmed)
                return Result.Failure(SellerApplicationErrors.DublicatedConfirmation);

            if (application.EmailConfirmationCode != request.Code ||
            application.EmailConfirmationCodeExpiration < DateTime.UtcNow)
            {
                return Result.Failure(SellerApplicationErrors.InvalidCode);
            }

            application.EmailConfirmed = true;
            application.EmailConfirmationCode = null;
            application.EmailConfirmationCodeExpiration = null;


            _context.SellerApplications.Update(application);
            await _context.SaveChangesAsync(cancellationToken);

            return Result.Success();
        }
    }
}
