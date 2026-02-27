namespace WearCast.Api.Features.Sellers.SellerManagers.CreateSellerManager
{
    public class CreateSellerManagerHandler(
        ApplicationDbContext context,
        UserManager<ApplicationUser> userManager,
        IMapper mapper
        ) : IRequestHandler<CreateSellerManagerRequest, Result>
    {
        private readonly ApplicationDbContext _context = context;
        private readonly UserManager<ApplicationUser> _userManager = userManager;
        private readonly IMapper _mapper = mapper;

        public async Task<Result> Handle(CreateSellerManagerRequest request, CancellationToken cancellationToken)
        {
            var existingPendingSellerManager = await _context.SellerApplications
                .Where(x => x.Status == Status.Pending && (x.ManagerEmail == request.Email || x.ManagerPhoneNumber == request.PhoneNumber))
                .Select(x => new { x.ManagerEmail, x.ManagerPhoneNumber })
                .FirstOrDefaultAsync(cancellationToken);

            if (existingPendingSellerManager != null)
            {
                if (existingPendingSellerManager.ManagerEmail.Equals(request.Email?.Trim(), StringComparison.OrdinalIgnoreCase))
                    return Result.Failure(SellerManagerErrors.EmailInUse);

                return Result.Failure(SellerManagerErrors.PhoneInUse);
            }

            var sellerIsExists = await _context.Sellers.AnyAsync(x => x.Id == request.SellerId, cancellationToken);

            if (!sellerIsExists)
                return Result.Failure(SellerErrors.SellerNotFound);

            var identityConflict = await _userManager.Users
                .Where(u => u.Email == request.Email || u.PhoneNumber == request.PhoneNumber)
                .Select(u => new { u.Email, u.PhoneNumber })
                .FirstOrDefaultAsync(cancellationToken);

            if (identityConflict != null)
            {
                if (identityConflict.Email.Equals(request.Email?.Trim(), StringComparison.OrdinalIgnoreCase))
                    return Result.Failure(SellerManagerErrors.EmailInUse);

                return Result.Failure(SellerManagerErrors.PhoneInUse);
            }

            await using var transaction = await _context.Database.BeginTransactionAsync(cancellationToken);

            try
            {
                var user = _mapper.Map<ApplicationUser>(request);

                var createUserResult = await _userManager.CreateAsync(user, request.Password);
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

                var sellerManager = new SellerManager
                {
                    UserId = user.Id,
                    SellerId = request.SellerId
                };

                await _context.SellerManagers.AddAsync(sellerManager, cancellationToken);
                await _context.SaveChangesAsync(cancellationToken);

                await transaction.CommitAsync(cancellationToken);

                return Result.Success();
            }
            catch (Exception ex)
            {
                await transaction.RollbackAsync(cancellationToken);
                return Result.Failure(new Error("SellerManager.CreationFailed", "An error occurred while creating the manager.", StatusCodes.Status500InternalServerError));
            }
        }
    }
}
