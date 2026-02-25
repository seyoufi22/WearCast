using System.Security.Cryptography;


namespace WearCast.Api.Features.AuthenticationManagement.Register
{
    public class RegisterCustomerHandler(
        UserManager<ApplicationUser> userManager,
        ILogger<RegisterCustomerHandler> logger,
        IMapper mapper,
        ApplicationDbContext context,
        EmailHelper emailHelper,
        ImageService imageService
        ) : IRequestHandler<RegisterCustomerRequest, Result<RegisterCustomerResponse>>
    {
        private readonly UserManager<ApplicationUser> _userManager = userManager;
        private readonly ILogger<RegisterCustomerHandler> _logger = logger;
        private readonly IMapper _mapper = mapper;
        private readonly ApplicationDbContext _context = context;
        private readonly EmailHelper _emailHelper = emailHelper;
        private readonly ImageService _imageService = imageService;

        public async Task<Result<RegisterCustomerResponse>> Handle(RegisterCustomerRequest request, CancellationToken cancellationToken)
        {
            var emailIsExists = await _userManager.Users.AnyAsync(x => x.Email == request.Email, cancellationToken);

            if (emailIsExists)
                return Result.Failure<RegisterCustomerResponse>(UserErrors.DublicatedEmail);

            var phoneNumberIsExists = await _userManager.Users.AnyAsync(x => x.PhoneNumber == request.PhoneNumber, cancellationToken);

            if (phoneNumberIsExists)
                return Result.Failure<RegisterCustomerResponse>(UserErrors.DublicatedPhoneNumber);

            string? profileImageUrl = null;
            if (request.ProfileImage != null)
            {
                var (isValid, errorMessage) = _imageService.Validate(request.ProfileImage);
                if (!isValid)
                {
                    return Result.Failure<RegisterCustomerResponse>(new Error("Image.Invalid", errorMessage, StatusCodes.Status400BadRequest));
                }

                profileImageUrl = await _imageService.UploadAsync(request.ProfileImage);
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

                    return Result.Failure<RegisterCustomerResponse>(new Error(error.Code, error.Description, StatusCodes.Status400BadRequest));
                }

                var roleResult = await _userManager.AddToRoleAsync(user, DefaultRoles.Customer);

                if (!roleResult.Succeeded)
                {
                    var error = roleResult.Errors.First();

                    return Result.Failure<RegisterCustomerResponse>(new Error(error.Code, error.Description, StatusCodes.Status400BadRequest));
                }

                var customer = new Customer
                {
                    UserId = user.Id,
                    ProfileImageUrl = profileImageUrl
                };

                await _context.Customers.AddAsync(customer, cancellationToken);

                await _context.SaveChangesAsync(cancellationToken);

                await transaction.CommitAsync(cancellationToken);

                await _emailHelper.SendConfirmationEmail(user, code);

                return Result.Success(new RegisterCustomerResponse(user.Id));
            }

            catch (Exception ex)
            {
                await transaction.RollbackAsync(cancellationToken);

                if (!string.IsNullOrEmpty(profileImageUrl))
                {
                    await _imageService.DeleteAsync(profileImageUrl);
                }

                _logger.LogError(ex, "An error occurred while registering the customer: {Email}", request.Email);

                return Result.Failure<RegisterCustomerResponse>(new Error("Registration.Failed", "An error occurred while registering the customer.", StatusCodes.Status500InternalServerError));
            }


        }
    }
}
