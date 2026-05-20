using System.Security.Cryptography;
using WearCast.Api.Features.AuthenticationManagement;
using WearCast.Api.Features.ShippingCompanies;


namespace WearCast.Api.Features.Drivers.CreateDriver
{
    public class CreateDriverHandler(
        ApplicationDbContext context,
        UserManager<ApplicationUser> userManager,
        IHttpContextAccessor httpContextAccessor,
        ImageService imageService,
        IMapper mapper,
        EmailHelper emailHelper,
        ILogger<CreateDriverHandler> logger
        ) : IRequestHandler<CreateDriverRequest, Result<CreateDriverResponse>>
    {
        private readonly ApplicationDbContext _context = context;
        private readonly UserManager<ApplicationUser> _userManager = userManager;
        private readonly IHttpContextAccessor _httpContextAccessor = httpContextAccessor;
        private readonly ImageService _imageService = imageService;
        private readonly IMapper _mapper = mapper;
        private readonly EmailHelper _emailHelper = emailHelper;
        private readonly ILogger<CreateDriverHandler> _logger = logger;

        public async Task<Result<CreateDriverResponse>> Handle(CreateDriverRequest request, CancellationToken cancellationToken)
        {
            var shippingCompanyId = await context.ShippingCompanies
                .AsNoTracking()
                .Where(x => !x.IsDeleted)
                .Select(s => (int?)s.Id)
                .FirstOrDefaultAsync(cancellationToken);

            if (shippingCompanyId == null)
                return Result.Failure<CreateDriverResponse>(new Error("ShippingCompany.NotFound", "Thier is no shipping company yet.", StatusCodes.Status404NotFound));


            var companyExists = await _context.ShippingCompanies.AnyAsync(x => x.Id == shippingCompanyId.Value, cancellationToken);
            if (!companyExists)
                return Result.Failure<CreateDriverResponse>(ShippingCompanyErrors.CompanyNotFound);


            bool isEmailUsedInSellerApplication = await _context.SellerApplications
                .AnyAsync(app =>
                    app.ManagerEmail == request.Email &&
                    (app.Status == Status.Pending || app.Status == Status.Approved),
                    cancellationToken);

            if (isEmailUsedInSellerApplication)
            {
                return Result.Failure<CreateDriverResponse>(UserErrors.DublicatedEmail);
            }

            var existingUser = await _userManager.Users
                .Where(x => x.Email == request.Email || x.PhoneNumber == request.PhoneNumber)
                .Select(x => new { x.Email, x.PhoneNumber })
                .FirstOrDefaultAsync(cancellationToken);

            if (existingUser != null)
            {
                if (existingUser.Email.Equals(request.Email?.Trim(), StringComparison.OrdinalIgnoreCase))
                    return Result.Failure<CreateDriverResponse>(UserErrors.DublicatedEmail);

                return Result.Failure<CreateDriverResponse>(UserErrors.DublicatedPhoneNumber);
            }

            var nationalIdExists = await _context.Drivers
                .AnyAsync(x => x.NationalId == request.NationalId, cancellationToken);

            if (nationalIdExists)
            {
                return Result.Failure<CreateDriverResponse>(DriverErrors.DuplicatedNationalId);
            }

            var profileImageUrl = await _imageService.UploadAsync(request.ProfileImage);

            var newDriverUser = _mapper.Map<ApplicationUser>(request);

            var code = RandomNumberGenerator.GetInt32(100000, 1000000).ToString();
            newDriverUser.EmailConfirmationCode = code;
            newDriverUser.EmailConfirmationCodeExpiration = DateTime.UtcNow.AddMinutes(60);

            await using var transaction = await _context.Database.BeginTransactionAsync(cancellationToken);

            try
            {
                var createUserResult = await _userManager.CreateAsync(newDriverUser, request.Password);

                if (!createUserResult.Succeeded)
                {
                    var error = createUserResult.Errors.First();
                    await _imageService.DeleteAsync(profileImageUrl);
                    return Result.Failure<CreateDriverResponse>(new Error(error.Code, error.Description, StatusCodes.Status400BadRequest));
                }

                var roleResult = await _userManager.AddToRoleAsync(newDriverUser, DefaultRoles.Driver);

                if (!roleResult.Succeeded)
                {
                    var error = roleResult.Errors.First();
                    await _imageService.DeleteAsync(profileImageUrl);
                    await transaction.RollbackAsync(cancellationToken);
                    return Result.Failure<CreateDriverResponse>(new Error(error.Code, error.Description, StatusCodes.Status400BadRequest));
                }

                var driver = _mapper.Map<Driver>(request);

                driver.ProfileImageUrl = profileImageUrl;
                driver.UserId = newDriverUser.Id;
                driver.ShippingCompanyId = shippingCompanyId.Value;


                await _context.Drivers.AddAsync(driver, cancellationToken);
                await _context.SaveChangesAsync(cancellationToken);

                await transaction.CommitAsync(cancellationToken);
            }
            catch (Exception ex)
            {
                await transaction.RollbackAsync(cancellationToken);
                await _imageService.DeleteAsync(profileImageUrl);

                _logger.LogError(ex, "Failed to create driver for email {Email}", request.Email);

                return Result.Failure<CreateDriverResponse>(new Error("Creating.Failed", "An error occurred while Creating the driver.", StatusCodes.Status500InternalServerError));
            }

            try
            {
                await _emailHelper.SendDriverConfirmationEmail(newDriverUser, code);
            }
            catch (Exception ex)
            {
                _logger.LogWarning(ex, "Driver registered but failed to send confirmation email to {Email}", request.Email);
            }

            return Result.Success(new CreateDriverResponse(newDriverUser.Id));
        }
    }
}