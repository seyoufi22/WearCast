using System.Security.Cryptography;
using WearCast.Api.Features.AuthenticationManagement;

namespace WearCast.Api.Features.Factories.CreateFactoryManager
{
    public class CreateFactoryManagerHandler(
        ApplicationDbContext context,
        UserManager<ApplicationUser> userManager,
        IMapper mapper,
        ILogger<CreateFactoryManagerHandler> logger
        ) : IRequestHandler<CreateFactoryManagerRequest, Result>
    {
        private readonly ApplicationDbContext _context = context;
        private readonly UserManager<ApplicationUser> _userManager = userManager;
        private readonly IMapper _mapper = mapper;
        private readonly ILogger<CreateFactoryManagerHandler> _logger = logger;

        public async Task<Result> Handle(CreateFactoryManagerRequest request, CancellationToken cancellationToken)
        {
            var factoryExists = await _context.Factories.AnyAsync(x => x.Id == request.FactoryId, cancellationToken);
            if (!factoryExists)
                return Result.Failure(FactoryErrors.FactoryNotFound);

            var existingUser = await _userManager.Users
                .Where(u => u.Email == request.Email || u.PhoneNumber == request.PhoneNumber)
                .Select(u => new { u.Email, u.PhoneNumber })
                .FirstOrDefaultAsync(cancellationToken);

            if (existingUser != null)
            {
                if (existingUser.Email.Equals(request.Email?.Trim(), StringComparison.OrdinalIgnoreCase))
                    return Result.Failure(UserErrors.DublicatedEmail);

                return Result.Failure(UserErrors.DublicatedPhoneNumber);
            }

            await using var transaction = await _context.Database.BeginTransactionAsync(cancellationToken);

            var user = _mapper.Map<ApplicationUser>(request);

            var code = RandomNumberGenerator.GetInt32(100000, 1000000).ToString();
            user.EmailConfirmationCode = code;
            user.EmailConfirmationCodeExpiration = DateTime.UtcNow.AddMinutes(60);

            try
            {

                var createUserResult = await _userManager.CreateAsync(user, request.Password);
                if (!createUserResult.Succeeded)
                {
                    var error = createUserResult.Errors.First();
                    return Result.Failure(new Error(error.Code, error.Description, StatusCodes.Status400BadRequest));
                }

                var roleResult = await _userManager.AddToRoleAsync(user, DefaultRoles.FactoryManager);
                if (!roleResult.Succeeded)
                {
                    await transaction.RollbackAsync(cancellationToken);

                    var error = roleResult.Errors.First();
                    return Result.Failure(new Error(error.Code, error.Description, StatusCodes.Status400BadRequest));
                }

                var factoryManager = new FactoryManager
                {
                    UserId = user.Id,
                    FactoryId = request.FactoryId
                };

                await _context.FactoryManagers.AddAsync(factoryManager, cancellationToken);
                await _context.SaveChangesAsync(cancellationToken);

                await transaction.CommitAsync(cancellationToken);

                return Result.Success();
            }
            catch (Exception ex)
            {
                await transaction.RollbackAsync(cancellationToken);

                _logger.LogError(ex, "Failed to create factory manager for email {Email}", request.Email);

                return Result.Failure(new Error("Creation.Failed", "An error occurred while creating the factory manager.", StatusCodes.Status500InternalServerError));
            }
        }
    }
}
