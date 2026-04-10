using System.Security.Cryptography;


namespace WearCast.Api.Features.Sellers.SellerManagers.CreateSellerManager
{
    public class CreateSellerManagerHandler(
        ApplicationDbContext context,
        UserManager<ApplicationUser> userManager,
        IHttpContextAccessor httpContextAccessor,
        IMapper mapper,
        EmailHelper emailHelper,
        ILogger<CreateSellerManagerHandler> logger
        ) : IRequestHandler<CreateSellerManagerRequest, Result>
    {
        private readonly ApplicationDbContext _context = context;
        private readonly UserManager<ApplicationUser> _userManager = userManager;
        private readonly IHttpContextAccessor _httpContextAccessor = httpContextAccessor;
        private readonly IMapper _mapper = mapper;
        private readonly EmailHelper _emailHelper = emailHelper;
        private readonly ILogger<CreateSellerManagerHandler> _logger = logger;

        public async Task<Result> Handle(CreateSellerManagerRequest request, CancellationToken cancellationToken)
        {
            var currentUser = _httpContextAccessor.HttpContext!.User;
            int targetSellerId;

            if (currentUser.IsSuperAdmin())
            {
                if (!request.ProvidedSellerId.HasValue)
                {
                    return Result.Failure(new Error("Validation.MissingId", "SuperAdmin must provide a target SellerId.", StatusCodes.Status400BadRequest));
                }
                targetSellerId = request.ProvidedSellerId.Value;
            }
            else
            {
                var sellerIdFromToken = currentUser.GetSellerId();
                if (!sellerIdFromToken.HasValue)
                {
                    return Result.Failure(new Error("User.InvalidToken", "Seller ID not found in token.", StatusCodes.Status401Unauthorized));
                }
                targetSellerId = sellerIdFromToken.Value;
            }

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

            var sellerIsExists = await _context.Sellers.AnyAsync(x => x.Id == targetSellerId, cancellationToken);

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

            var newManagerUser = _mapper.Map<ApplicationUser>(request);

            var code = RandomNumberGenerator.GetInt32(100000, 1000000).ToString();
            newManagerUser.EmailConfirmationCode = code;
            newManagerUser.EmailConfirmationCodeExpiration = DateTime.UtcNow.AddMinutes(60);

            await using var transaction = await _context.Database.BeginTransactionAsync(cancellationToken);

            try
            {
                var createUserResult = await _userManager.CreateAsync(newManagerUser, request.Password);
                if (!createUserResult.Succeeded)
                {
                    var error = createUserResult.Errors.First();
                    return Result.Failure(new Error(error.Code, error.Description, StatusCodes.Status400BadRequest));
                }

                var roleResult = await _userManager.AddToRoleAsync(newManagerUser, DefaultRoles.SellerManager);
                if (!roleResult.Succeeded)
                {
                    await transaction.RollbackAsync(cancellationToken);
                    var error = roleResult.Errors.First();
                    return Result.Failure(new Error(error.Code, error.Description, StatusCodes.Status400BadRequest));
                }

                var sellerManager = new SellerManager
                {
                    UserId = newManagerUser.Id,
                    SellerId = targetSellerId
                };

                await _context.SellerManagers.AddAsync(sellerManager, cancellationToken);
                await _context.SaveChangesAsync(cancellationToken);

                await transaction.CommitAsync(cancellationToken);
            }
            catch (Exception ex)
            {
                await transaction.RollbackAsync(cancellationToken);
                _logger.LogError(ex, "Failed to create seller manager for email {Email}", request.Email);
                return Result.Failure(new Error("SellerManager.CreationFailed", "An error occurred while creating the manager.", StatusCodes.Status500InternalServerError));
            }

            try
            {
                await _emailHelper.SendConfirmationEmail(newManagerUser, code);
            }
            catch (Exception ex)
            {
                _logger.LogWarning(ex, "Seller Manager created but failed to send email to {Email}", request.Email);
            }

            return Result.Success();
        }
    }
}