namespace WearCast.Api.Features.Sellers.SellerApplications.RejectSellerApplication
{
    public class RejectSellerApplicationHandler(
        ApplicationDbContext context,
        EmailHelper emailHelper
        ) : IRequestHandler<RejectSellerApplicationRequest, Result>
    {
        private readonly ApplicationDbContext _context = context;
        private readonly EmailHelper _emailHelper = emailHelper;

        public async Task<Result> Handle(RejectSellerApplicationRequest request, CancellationToken cancellationToken)
        {
            var application = await _context.SellerApplications.FirstOrDefaultAsync(x => x.ManagerEmail == request.Email, cancellationToken);

            if (application == null)
                return Result.Failure(SellerApplicationErrors.ApplicationNotFound);

            if (application.Status != Status.Pending)
                return Result.Failure(SellerApplicationErrors.ApplicationNotPending);

            application.Status = Status.Rejected;
            application.RejectionReason = request.RejectionReason;

            _context.SellerApplications.Update(application);
            await _context.SaveChangesAsync(cancellationToken);

            await _emailHelper.SendSellerApplicationRejectedEmail(application);

            return Result.Success();
        }
    }
}
