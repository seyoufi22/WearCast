using System.Security.Cryptography;
using WearCast.Api.Features.AuthenticationManagement;

namespace WearCast.Api.Features.Factories.CreateFactory
{
    public class CreateFactoryHandler(
        ApplicationDbContext context,
        UserManager<ApplicationUser> userManager,
        ImageService imageService,
        IMapper mapper,
        EmailHelper emailHelper,
        ILogger<CreateFactoryHandler> logger
        ) : IRequestHandler<CreateFactoryRequest, Result<CreateFactoryResponse>>
    {
        private readonly ApplicationDbContext _context = context;
        private readonly UserManager<ApplicationUser> _userManager = userManager;
        private readonly ImageService _imageService = imageService;
        private readonly IMapper _mapper = mapper;
        private readonly EmailHelper _emailHelper = emailHelper;
        private readonly ILogger<CreateFactoryHandler> _logger = logger;

        public async Task<Result<CreateFactoryResponse>> Handle(CreateFactoryRequest request, CancellationToken cancellationToken)
        {
            var existingUser = await _userManager.Users
                .Where(u => u.Email == request.ManagerEmail || u.PhoneNumber == request.ManagerPhoneNumber)
                .Select(u => new { u.Email, u.PhoneNumber })
                .FirstOrDefaultAsync(cancellationToken);

            if (existingUser != null)
            {
                if (existingUser.Email.Equals(request.ManagerEmail?.Trim(), StringComparison.OrdinalIgnoreCase))
                    return Result.Failure<CreateFactoryResponse>(UserErrors.DublicatedEmail);

                return Result.Failure<CreateFactoryResponse>(UserErrors.DublicatedPhoneNumber);
            }

            var factoryLogoUrl = await _imageService.UploadAsync(request.FactoryLogo);

            var user = _mapper.Map<ApplicationUser>(request);

            var code = RandomNumberGenerator.GetInt32(100000, 1000000).ToString();
            user.EmailConfirmationCode = code;
            user.EmailConfirmationCodeExpiration = DateTime.UtcNow.AddMinutes(60);

            await using var transaction = await _context.Database.BeginTransactionAsync(cancellationToken);
            try
            {
                var createUserResult = await _userManager.CreateAsync(user, request.ManagerPassword);
                if (!createUserResult.Succeeded)
                {
                    await _imageService.DeleteAsync(factoryLogoUrl);
                    var error = createUserResult.Errors.First();
                    return Result.Failure<CreateFactoryResponse>(new Error(error.Code, error.Description, StatusCodes.Status400BadRequest));
                }

                var roleResult = await _userManager.AddToRoleAsync(user, DefaultRoles.FactoryManager);
                if (!roleResult.Succeeded)
                {
                    await _imageService.DeleteAsync(factoryLogoUrl);
                    await transaction.RollbackAsync(cancellationToken);
                    var error = roleResult.Errors.First();
                    return Result.Failure<CreateFactoryResponse>(new Error(error.Code, error.Description, StatusCodes.Status400BadRequest));
                }

                var factory = _mapper.Map<Factory>(request);
                factory.LogoUrl = factoryLogoUrl;

                await _context.Factories.AddAsync(factory, cancellationToken);
                await _context.SaveChangesAsync(cancellationToken);

                var factoryManager = new FactoryManager
                {
                    UserId = user.Id,
                    FactoryId = factory.Id
                };

                await _context.FactoryManagers.AddAsync(factoryManager, cancellationToken);
                await _context.SaveChangesAsync(cancellationToken);

                await transaction.CommitAsync(cancellationToken);
            }
            catch (Exception ex)
            {
                await transaction.RollbackAsync(cancellationToken);
                await _imageService.DeleteAsync(factoryLogoUrl);

                _logger.LogError(ex, "Failed to create factory for email {Email}", request.ManagerEmail);

                return Result.Failure<CreateFactoryResponse>(new Error("Creation.Failed", "An error occurred while Creating the Factory.", StatusCodes.Status500InternalServerError));
            }

            try
            {
                await _emailHelper.SendConfirmationEmail(user, code);
            }
            catch (Exception ex)
            {
                _logger.LogWarning(ex, "Factory created but failed to send email to {Email}", request.ManagerEmail);
            }

            return Result.Success(new CreateFactoryResponse(user.Id));
        }
    }
}