using System.Security.Cryptography;
using WearCast.Api.Features.AuthenticationManagement;
using WearCast.Api.Features.ShippingCompanies;

namespace WearCast.Api.Features.Drivers.CreateDriver
{
    public class CreateDriverHandler(
        ApplicationDbContext context,
        UserManager<ApplicationUser> userManager,
        ImageService imageService,
        IMapper mapper,
        EmailHelper emailHelper,
        ILogger<Driver> logger
        ) : IRequestHandler<CreateDriverRequest, Result<CreateDriverResponse>>
    {
        private readonly ApplicationDbContext _context = context;
        private readonly UserManager<ApplicationUser> _userManager = userManager;
        private readonly ImageService _imageService = imageService;
        private readonly IMapper _mapper = mapper;
        private readonly EmailHelper _emailHelper = emailHelper;
        private readonly ILogger<Driver> _logger = logger;

        public async Task<Result<CreateDriverResponse>> Handle(CreateDriverRequest request, CancellationToken cancellationToken)
        {
            var companyExists = await _context.ShippingCompanies.AnyAsync(x => x.Id == request.ShippingCompanyId, cancellationToken);
            if (!companyExists)
                return Result.Failure<CreateDriverResponse>(ShippingCompanyErrors.CompanyNotFound);

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

                    await _imageService.DeleteAsync(profileImageUrl);

                    return Result.Failure<CreateDriverResponse>(new Error(error.Code, error.Description, StatusCodes.Status400BadRequest));
                }

                var roleResult = await _userManager.AddToRoleAsync(user, DefaultRoles.Driver);

                if (!roleResult.Succeeded)
                {
                    var error = roleResult.Errors.First();

                    await _imageService.DeleteAsync(profileImageUrl);

                    await transaction.RollbackAsync(cancellationToken);

                    return Result.Failure<CreateDriverResponse>(new Error(error.Code, error.Description, StatusCodes.Status400BadRequest));
                }

                var driver = _mapper.Map<Driver>(request);

                driver.ProfileImageUrl = profileImageUrl;
                driver.UserId = user.Id;


                await _context.Drivers.AddAsync(driver, cancellationToken);
                await _context.SaveChangesAsync(cancellationToken);

                await transaction.CommitAsync(cancellationToken);
            }
            catch (Exception ex)
            {
                await transaction.RollbackAsync(cancellationToken);


                await _imageService.DeleteAsync(profileImageUrl);


                return Result.Failure<CreateDriverResponse>(new Error("Creating.Failed", "An error occurred while Creating the driver.", StatusCodes.Status500InternalServerError));
            }
            try
            {
                await _emailHelper.SendConfirmationEmail(user, code);
            }
            catch (Exception ex)
            {
                _logger.LogWarning(ex, "Driver registered but failed to send confirmation email to {Email}", request.Email);
            }

            return Result.Success(new CreateDriverResponse(user.Id));
        }
    }
}
