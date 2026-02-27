namespace WearCast.Api.Features.Sellers.SellerApplications.ApproveSellerApplication
{
    public class ApproveSellerApplicationHandler(
        ApplicationDbContext context,
        UserManager<ApplicationUser> userManager,
        IMapper mapper,
        EmailHelper emailHelper
        ) : IRequestHandler<ApproveSellerApplicationRequest, Result>
    {
        private readonly ApplicationDbContext _context = context;
        private readonly UserManager<ApplicationUser> _userManager = userManager;
        private readonly IMapper _mapper = mapper;
        private readonly EmailHelper _emailHelper = emailHelper;

        public async Task<Result> Handle(ApproveSellerApplicationRequest request, CancellationToken cancellationToken)
        {
            var application = await _context.SellerApplications
                .FirstOrDefaultAsync(x => x.ManagerEmail == request.Email, cancellationToken);

            if (application == null)
                return Result.Failure(SellerApplicationErrors.ApplicationNotFound);

            if (application.Status != Status.Pending)
                return Result.Failure(SellerApplicationErrors.ApplicationNotPending);

            if (!application.ManagerEmailConfirmed)
                return Result.Failure(SellerManagerErrors.EmailNotConfirmed);

            var identityConflict = await _userManager.Users
                .Where(u => u.Email == application.ManagerEmail || u.PhoneNumber == application.ManagerPhoneNumber)
                .Select(u => new { u.Email, u.PhoneNumber })
                .FirstOrDefaultAsync(cancellationToken);

            if (identityConflict != null)
            {
                if (identityConflict.Email == application.ManagerEmail)
                    return Result.Failure(SellerManagerErrors.EmailInUse);

                return Result.Failure(SellerManagerErrors.PhoneInUse);
            }

            await using var transaction = await _context.Database.BeginTransactionAsync(cancellationToken);

            try
            {
                var user = _mapper.Map<ApplicationUser>(application);

                var createUserResult = await _userManager.CreateAsync(user);
                if (!createUserResult.Succeeded)
                {
                    var error = createUserResult.Errors.First();
                    return Result.Failure(new Error(error.Code, error.Description, StatusCodes.Status400BadRequest));
                }


                var roleResult = await _userManager.AddToRoleAsync(user, DefaultRoles.Seller);
                if (!roleResult.Succeeded)
                {
                    var error = roleResult.Errors.First();
                    return Result.Failure(new Error(error.Code, error.Description, StatusCodes.Status400BadRequest));
                }

                var seller = _mapper.Map<Seller>(application);
                await _context.Sellers.AddAsync(seller, cancellationToken);
                await _context.SaveChangesAsync(cancellationToken);

                var sellerManager = new SellerManager
                {
                    UserId = user.Id,
                    SellerId = seller.Id
                };
                await _context.SellerManagers.AddAsync(sellerManager, cancellationToken);

                application.Status = Status.Approved;
                _context.SellerApplications.Update(application);

                await _context.SaveChangesAsync(cancellationToken);
                await transaction.CommitAsync(cancellationToken);

                await _emailHelper.SendSellerApplicationApprovedEmail(application);

                return Result.Success();
            }
            catch (Exception ex)
            {
                await transaction.RollbackAsync(cancellationToken);

                return Result.Failure(SellerApplicationErrors.ApproveFailed);
            }
        }
    }
}