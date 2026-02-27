namespace WearCast.Api.Features.Sellers.SellerApplications.SellerManagerConfirmEmail
{
    public class SellerManagerConfirmEmailHandler(
        ApplicationDbContext context
        ) : IRequestHandler<SellerManagerConfirmEmailRequest, Result>
    {
        private readonly ApplicationDbContext _context = context;

        public async Task<Result> Handle(SellerManagerConfirmEmailRequest request, CancellationToken cancellationToken)
        {
            var application = await _context.SellerApplications.FirstOrDefaultAsync(x => x.ManagerEmail == request.Email, cancellationToken);

            if (application == null)
                return Result.Failure(SellerApplicationErrors.ApplicationNotFound);

            if (application.ManagerEmailConfirmed)
                return Result.Failure(SellerApplicationErrors.DublicatedConfirmation);

            if (application.ManagerEmailConfirmationCode != request.Code ||
            application.ManagerEmailConfirmationCodeExpiration < DateTime.UtcNow)
            {
                return Result.Failure(SellerApplicationErrors.InvalidCode);
            }

            application.ManagerEmailConfirmed = true;
            application.ManagerEmailConfirmationCode = null;
            application.ManagerEmailConfirmationCodeExpiration = null;


            _context.SellerApplications.Update(application);
            await _context.SaveChangesAsync(cancellationToken);

            return Result.Success();
        }
    }
}
