using System.Security.Cryptography;
using WearCast.Api.Features.AuthenticationManagement;

namespace WearCast.Api.Features.ShippingCompanies.CreateShippingCompany
{
    public class CreateShippingCompanyHandler(
        ApplicationDbContext context,
        UserManager<ApplicationUser> userManager,
        IMapper mapper,
        ImageService imageService,
        EmailHelper emailHelper,
        ILogger<CreateShippingCompanyHandler> logger
        ) : IRequestHandler<CreateShippingCompanyRequest, Result<CreateShippingCompanyResponse>>
    {
        private readonly ApplicationDbContext _context = context;
        private readonly UserManager<ApplicationUser> _userManager = userManager;
        private readonly IMapper _mapper = mapper;
        private readonly ImageService _imageService = imageService;
        private readonly EmailHelper _emailHelper = emailHelper;
        private readonly ILogger<CreateShippingCompanyHandler> _logger = logger;

        public async Task<Result<CreateShippingCompanyResponse>> Handle(CreateShippingCompanyRequest request, CancellationToken cancellationToken)
        {
            bool companyExists = await _context.ShippingCompanies.AnyAsync(cancellationToken);
            if (companyExists)
            {
                return Result.Failure<CreateShippingCompanyResponse>(
                    new Error("ShippingCompany.AlreadyExists", "A shipping company already exists in the system. Only one company is allowed.", StatusCodes.Status400BadRequest)
                );
            }

            var existingUser = await _userManager.Users
                .Where(u => u.Email == request.ManagerEmail || u.PhoneNumber == request.ManagerPhoneNumber)
                .Select(u => new { u.Email, u.PhoneNumber })
                .FirstOrDefaultAsync(cancellationToken);

            if (existingUser != null)
            {
                if (existingUser.Email.Equals(request.ManagerEmail?.Trim(), StringComparison.OrdinalIgnoreCase))
                    return Result.Failure<CreateShippingCompanyResponse>(UserErrors.DublicatedEmail);

                return Result.Failure<CreateShippingCompanyResponse>(UserErrors.DublicatedPhoneNumber);
            }

            var companyLogoUrl = await _imageService.UploadAsync(request.CompanyLogo);

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
                    await _imageService.DeleteAsync(companyLogoUrl);

                    var error = createUserResult.Errors.First();
                    return Result.Failure<CreateShippingCompanyResponse>(new Error(error.Code, error.Description, StatusCodes.Status400BadRequest));
                }

                var roleResult = await _userManager.AddToRoleAsync(user, DefaultRoles.ShippingCompanyManager);
                if (!roleResult.Succeeded)
                {
                    await _imageService.DeleteAsync(companyLogoUrl);

                    await transaction.RollbackAsync(cancellationToken);

                    var error = roleResult.Errors.First();
                    return Result.Failure<CreateShippingCompanyResponse>(new Error(error.Code, error.Description, StatusCodes.Status400BadRequest));
                }

                var shippingCompany = _mapper.Map<ShippingCompany>(request);
                shippingCompany.LogoUrl = companyLogoUrl;

                await _context.ShippingCompanies.AddAsync(shippingCompany, cancellationToken);
                await _context.SaveChangesAsync(cancellationToken);

                var shippingCompanyManager = new ShippingCompanyManager
                {
                    UserId = user.Id,
                    ShippingCompanyId = shippingCompany.Id
                };

                await _context.ShippingCompanyManagers.AddAsync(shippingCompanyManager, cancellationToken);
                await _context.SaveChangesAsync(cancellationToken);

                await transaction.CommitAsync(cancellationToken);
            }
            catch (Exception ex)
            {
                await _imageService.DeleteAsync(companyLogoUrl);

                await transaction.RollbackAsync(cancellationToken);

                return Result.Failure<CreateShippingCompanyResponse>(new Error("Creation.Failed", "An error occurred while Creating the Shipping Company.", StatusCodes.Status500InternalServerError));
            }
            try
            {
                await _emailHelper.SendShippingCompanyManagerConfirmationEmail(user, code);
            }
            catch (Exception ex)
            {
                _logger.LogWarning(ex, "Shipping Company created but failed to send email to {Email}", request.ManagerEmail);
            }

            return Result.Success(new CreateShippingCompanyResponse(user.Id));
        }
    }
}
