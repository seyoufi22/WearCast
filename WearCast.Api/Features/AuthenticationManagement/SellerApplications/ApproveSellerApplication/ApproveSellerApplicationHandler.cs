

namespace WearCast.Api.Features.AuthenticationManagement.SellerApplications.ApproveSellerApplication
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
            var application = await _context.SellerApplications.FirstOrDefaultAsync(x => x.Email == request.Email, cancellationToken);

            if (application == null)
                return Result.Failure(SellerApplicationErrors.ApplicationNotFound);

            if (application.Status != Status.Pending)
                return Result.Failure(SellerApplicationErrors.ApplicationNotPending);

            if (!application.EmailConfirmed)
                return Result.Failure(SellerApplicationErrors.EmailNotConfirmed);

            var emailIsExists = await _userManager.Users.AnyAsync(x => x.Email == application.Email, cancellationToken);

            if (emailIsExists)
                return Result.Failure(SellerApplicationErrors.EmailInUse);

            var phoneNumberIsExists = await _userManager.Users.AnyAsync(x => x.PhoneNumber == application.PhoneNumber, cancellationToken);

            if (phoneNumberIsExists)
                return Result.Failure(SellerApplicationErrors.PhoneInUse);

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

                seller.UserId = user.Id;

                await context.Sellers.AddAsync(seller, cancellationToken);

                application.Status = Status.Approved;

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
