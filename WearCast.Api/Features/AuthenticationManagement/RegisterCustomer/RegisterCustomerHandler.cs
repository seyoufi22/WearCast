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

            bool isEmailUsedInSellerApplication = await _context.SellerApplications
                            .AnyAsync(app =>
                                app.ManagerEmail == request.Email &&
                                (app.Status == Status.Pending || app.Status == Status.Approved),
                                cancellationToken);

            if (isEmailUsedInSellerApplication)
            {
                return Result.Failure<RegisterCustomerResponse>(UserErrors.DublicatedEmail);
            }

            var existingUsers = await _userManager.Users
                 .IgnoreQueryFilters()
                 .Where(x => x.Email == request.Email || x.PhoneNumber == request.PhoneNumber)
                 .Select(x => new { x.Email, x.PhoneNumber, x.IsDeleted })
                 .ToListAsync(cancellationToken);

            if (existingUsers.Any())
            {
                var emailMatch = existingUsers.FirstOrDefault(x => x.Email.Equals(request.Email?.Trim(), StringComparison.OrdinalIgnoreCase));
                if (emailMatch != null)
                {
                    if (emailMatch.IsDeleted)
                        return Result.Failure<RegisterCustomerResponse>(UserErrors.AccountDeleted);
                    else
                        return Result.Failure<RegisterCustomerResponse>(UserErrors.DublicatedEmail);
                }

                var phoneMatch = existingUsers.FirstOrDefault(x => x.PhoneNumber == request.PhoneNumber);
                if (phoneMatch != null && !phoneMatch.IsDeleted)
                {
                    return Result.Failure<RegisterCustomerResponse>(UserErrors.DublicatedPhoneNumber);
                }
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
                    await _imageService.DeleteAsync(profileImageUrl);

                    var error = createUserResult.Errors.First();

                    return Result.Failure<RegisterCustomerResponse>(new Error(error.Code, error.Description, StatusCodes.Status400BadRequest));
                }

                var roleResult = await _userManager.AddToRoleAsync(user, DefaultRoles.Customer);

                if (!roleResult.Succeeded)
                {
                    await transaction.RollbackAsync(cancellationToken);

                    await _imageService.DeleteAsync(profileImageUrl);

                    var error = roleResult.Errors.First();

                    return Result.Failure<RegisterCustomerResponse>(new Error(error.Code, error.Description, StatusCodes.Status400BadRequest));
                }

                var customer = _mapper.Map<Customer>(request);

                customer.UserId = user.Id;
                customer.ProfileImageUrl = profileImageUrl;

                await _context.Customers.AddAsync(customer, cancellationToken);

                await _context.SaveChangesAsync(cancellationToken);

                await transaction.CommitAsync(cancellationToken);
            }

            catch (Exception ex)
            {
                await transaction.RollbackAsync(cancellationToken);


                await _imageService.DeleteAsync(profileImageUrl);

                _logger.LogError(ex, "Registration failed");

                _logger.LogError(ex, "An error occurred while registering the customer: {Email}", request.Email);

                return Result.Failure<RegisterCustomerResponse>(new Error("Registration.Failed", "An error occurred while registering the customer.", StatusCodes.Status500InternalServerError));
            }

            try
            {
                await _emailHelper.SendCustomerConfirmationEmail(user, code);
            }
            catch (Exception ex)
            {

                _logger.LogWarning(ex, "Customer registered but failed to send confirmation email to {Email}", request.Email);
            }

            return Result.Success(new RegisterCustomerResponse(user.Id));

        }
    }
}
