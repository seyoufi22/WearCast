using System.Security.Cryptography;
using WearCast.Api.Features.AuthenticationManagement;

namespace WearCast.Api.Features.ShippingCompanies.CreateShippingCompanyManager
{
    public class CreateShippingCompanyManagerHandler(
        ApplicationDbContext context,
        UserManager<ApplicationUser> userManager,
        IMapper mapper,
        EmailHelper emailHelper,
        ILogger<CreateShippingCompanyManagerHandler> logger
        ) : IRequestHandler<CreateShippingCompanyManagerRequest, Result<CreateShippingCompanyManagerResponse>>
    {
        private readonly ApplicationDbContext _context = context;
        private readonly UserManager<ApplicationUser> _userManager = userManager;
        private readonly IMapper _mapper = mapper;
        private readonly EmailHelper _emailHelper = emailHelper;
        private readonly ILogger<CreateShippingCompanyManagerHandler> _logger = logger;

        public async Task<Result<CreateShippingCompanyManagerResponse>> Handle(CreateShippingCompanyManagerRequest request, CancellationToken cancellationToken)
        {
            var companyExists = await _context.ShippingCompanies.AnyAsync(x => x.Id == request.ShippingCompanyId, cancellationToken);
            if (!companyExists)
                return Result.Failure<CreateShippingCompanyManagerResponse>(ShippingCompanyErrors.CompanyNotFound);

            var existingUser = await _userManager.Users
                .Where(u => u.Email == request.Email || u.PhoneNumber == request.PhoneNumber)
                .Select(u => new { u.Email, u.PhoneNumber })
                .FirstOrDefaultAsync(cancellationToken);

            if (existingUser != null)
            {
                if (existingUser.Email.Equals(request.Email?.Trim(), StringComparison.OrdinalIgnoreCase))
                    return Result.Failure<CreateShippingCompanyManagerResponse>(UserErrors.DublicatedEmail);

                return Result.Failure<CreateShippingCompanyManagerResponse>(UserErrors.DublicatedPhoneNumber);
            }

            var user = _mapper.Map<ApplicationUser>(request);
            var code = RandomNumberGenerator.GetInt32(100000, 1000000).ToString();
            user.EmailConfirmationCode = code;
            user.EmailConfirmationCodeExpiration = DateTime.UtcNow.AddMinutes(60);

            await using var transaction = await _context.Database.BeginTransactionAsync(cancellationToken);

            try
            {
                var createUserResult = await _userManager.CreateAsync(user, request.Password);
                if (!createUserResult.Succeeded)
                {
                    var error = createUserResult.Errors.First();
                    return Result.Failure<CreateShippingCompanyManagerResponse>(new Error(error.Code, error.Description, StatusCodes.Status400BadRequest));
                }

                var roleResult = await _userManager.AddToRoleAsync(user, DefaultRoles.ShippingCompanyManager);
                if (!roleResult.Succeeded)
                {
                    await transaction.RollbackAsync(cancellationToken);
                    var error = roleResult.Errors.First();
                    return Result.Failure<CreateShippingCompanyManagerResponse>(new Error(error.Code, error.Description, StatusCodes.Status400BadRequest));
                }

                var companyManager = new ShippingCompanyManager
                {
                    UserId = user.Id,
                    ShippingCompanyId = request.ShippingCompanyId
                };

                await _context.ShippingCompanyManagers.AddAsync(companyManager, cancellationToken);
                await _context.SaveChangesAsync(cancellationToken);

                await transaction.CommitAsync(cancellationToken);
            }
            catch (Exception ex)
            {
                await transaction.RollbackAsync(cancellationToken);
                _logger.LogError(ex, "Failed to create shipping company manager for email {Email}", request.Email);
                return Result.Failure<CreateShippingCompanyManagerResponse>(new Error("Creation.Failed", "An error occurred while creating the shipping company manager.", StatusCodes.Status500InternalServerError));
            }

            try
            {
                await _emailHelper.SendConfirmationEmail(user, code);
            }
            catch (Exception ex)
            {
                _logger.LogWarning(ex, "Manager created but failed to send confirmation email to {Email}", request.Email);
            }

            return Result.Success(new CreateShippingCompanyManagerResponse(user.Id));
        }
    }
}
