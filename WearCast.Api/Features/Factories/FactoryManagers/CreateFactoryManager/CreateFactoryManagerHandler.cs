using System.Security.Cryptography;
using WearCast.Api.Features.AuthenticationManagement;

namespace WearCast.Api.Features.Factories.FactoryManagers.CreateFactoryManager
{
    public class CreateFactoryManagerHandler(
        ApplicationDbContext context,
        UserManager<ApplicationUser> userManager,
        IHttpContextAccessor httpContextAccessor,
        IMapper mapper,
        EmailHelper emailHelper,
        ILogger<CreateFactoryManagerHandler> logger
        ) : IRequestHandler<CreateFactoryManagerRequest, Result<CreateFactoryManagerResponse>>
    {
        private readonly ApplicationDbContext _context = context;
        private readonly UserManager<ApplicationUser> _userManager = userManager;
        private readonly IHttpContextAccessor _httpContextAccessor = httpContextAccessor;
        private readonly IMapper _mapper = mapper;
        private readonly EmailHelper _emailHelper = emailHelper;
        private readonly ILogger<CreateFactoryManagerHandler> _logger = logger;

        public async Task<Result<CreateFactoryManagerResponse>> Handle(CreateFactoryManagerRequest request, CancellationToken cancellationToken)
        {
            var currentUser = _httpContextAccessor.HttpContext!.User;

            int targetFactoryId;

            if (currentUser.IsSuperAdmin())
            {
                if (!request.ProvidedFactoryId.HasValue)
                {
                    return Result.Failure<CreateFactoryManagerResponse>(new Error("Validation.MissingId", "SuperAdmin must provide a target ProviderId to delete.", StatusCodes.Status400BadRequest));
                }

                targetFactoryId = request.ProvidedFactoryId.Value;
            }
            else
            {
                targetFactoryId = currentUser.GetFactoryId()!.Value;
            }

            var factoryExists = await _context.Factories.AnyAsync(x => x.Id == targetFactoryId, cancellationToken);
            if (!factoryExists)
                return Result.Failure<CreateFactoryManagerResponse>(FactoryErrors.FactoryNotFound);

            var existingUser = await _userManager.Users
                .Where(u => u.Email == request.Email || u.PhoneNumber == request.PhoneNumber)
                .Select(u => new { u.Email, u.PhoneNumber })
                .FirstOrDefaultAsync(cancellationToken);

            if (existingUser != null)
            {
                if (existingUser.Email.Equals(request.Email?.Trim(), StringComparison.OrdinalIgnoreCase))
                    return Result.Failure<CreateFactoryManagerResponse>(UserErrors.DublicatedEmail);

                return Result.Failure<CreateFactoryManagerResponse>(UserErrors.DublicatedPhoneNumber);
            }

            await using var transaction = await _context.Database.BeginTransactionAsync(cancellationToken);


            var newManagerUser = _mapper.Map<ApplicationUser>(request);

            var code = RandomNumberGenerator.GetInt32(100000, 1000000).ToString();
            newManagerUser.EmailConfirmationCode = code;
            newManagerUser.EmailConfirmationCodeExpiration = DateTime.UtcNow.AddMinutes(60);

            try
            {
                var createUserResult = await _userManager.CreateAsync(newManagerUser, request.Password);
                if (!createUserResult.Succeeded)
                {
                    var error = createUserResult.Errors.First();
                    return Result.Failure<CreateFactoryManagerResponse>(new Error(error.Code, error.Description, StatusCodes.Status400BadRequest));
                }

                var roleResult = await _userManager.AddToRoleAsync(newManagerUser, DefaultRoles.FactoryManager);
                if (!roleResult.Succeeded)
                {
                    await transaction.RollbackAsync(cancellationToken);

                    var error = roleResult.Errors.First();
                    return Result.Failure<CreateFactoryManagerResponse>(new Error(error.Code, error.Description, StatusCodes.Status400BadRequest));
                }

                var factoryManager = new FactoryManager
                {
                    UserId = newManagerUser.Id,
                    FactoryId = targetFactoryId
                };

                await _context.FactoryManagers.AddAsync(factoryManager, cancellationToken);
                await _context.SaveChangesAsync(cancellationToken);

                await transaction.CommitAsync(cancellationToken);
            }
            catch (Exception ex)
            {
                await transaction.RollbackAsync(cancellationToken);

                _logger.LogError(ex, "Failed to create factory manager for email {Email}", request.Email);

                return Result.Failure<CreateFactoryManagerResponse>(new Error("Creation.Failed", "An error occurred while creating the factory manager.", StatusCodes.Status500InternalServerError));
            }

            try
            {
                await _emailHelper.SendFactoryManagerConfirmationEmail(newManagerUser, code);
            }
            catch (Exception ex)
            {
                _logger.LogWarning(ex, "Factory Manager created but failed to send email to {Email}", request.Email);
            }

            // 5. Moved the Success return outside the try-catch block
            return Result.Success(new CreateFactoryManagerResponse(newManagerUser.Id));
        }
    }
}