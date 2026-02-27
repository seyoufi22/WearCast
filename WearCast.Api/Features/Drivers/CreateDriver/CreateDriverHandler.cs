using WearCast.Api.Features.AuthenticationManagement;

namespace WearCast.Api.Features.Drivers.CreateDriver
{
    public class CreateDriverHandler(
        ApplicationDbContext context,
        UserManager<ApplicationUser> userManager,
        ImageService imageService,
        IMapper mapper
        ) : IRequestHandler<CreateDriverRequest, Result>
    {
        private readonly ApplicationDbContext _context = context;
        private readonly UserManager<ApplicationUser> _userManager = userManager;
        private readonly ImageService _imageService = imageService;
        private readonly IMapper _mapper = mapper;

        public async Task<Result> Handle(CreateDriverRequest request, CancellationToken cancellationToken)
        {
            var existingUser = await _userManager.Users
                .Where(x => x.Email == request.Email || x.PhoneNumber == request.PhoneNumber)
                .Select(x => new { x.Email, x.PhoneNumber })
                .FirstOrDefaultAsync(cancellationToken);

            if (existingUser != null)
            {
                if (existingUser.Email == request.Email)
                    return Result.Failure(UserErrors.DublicatedEmail);

                return Result.Failure(UserErrors.DublicatedPhoneNumber);
            }

            var nationalIdExists = await _context.Drivers
                .AnyAsync(x => x.NationalId == request.NationalId, cancellationToken);

            if (nationalIdExists)
            {
                return Result.Failure(DriverErrors.DublicatedNationalId);
            }

            var profileImageUrl = await _imageService.UploadAsync(request.ProfileImage);

            var user = _mapper.Map<ApplicationUser>(request);

            await using var transaction = await _context.Database.BeginTransactionAsync(cancellationToken);

            try
            {
                var createUserResult = await _userManager.CreateAsync(user, request.Password);

                if (!createUserResult.Succeeded)
                {
                    var error = createUserResult.Errors.First();

                    await _imageService.DeleteAsync(profileImageUrl);

                    return Result.Failure(new Error(error.Code, error.Description, StatusCodes.Status400BadRequest));
                }

                var roleResult = await _userManager.AddToRoleAsync(user, DefaultRoles.Driver);

                if (!roleResult.Succeeded)
                {
                    var error = roleResult.Errors.First();

                    await _imageService.DeleteAsync(profileImageUrl);

                    await transaction.RollbackAsync(cancellationToken);

                    return Result.Failure(new Error(error.Code, error.Description, StatusCodes.Status400BadRequest));
                }

                var driver = _mapper.Map<Driver>(request);

                driver.ProfileImageUrl = profileImageUrl;
                driver.UserId = user.Id;


                await _context.Drivers.AddAsync(driver, cancellationToken);
                await _context.SaveChangesAsync(cancellationToken);
                await transaction.CommitAsync(cancellationToken);

                return Result.Success();
            }
            catch (Exception ex)
            {
                await transaction.RollbackAsync(cancellationToken);


                await _imageService.DeleteAsync(profileImageUrl);


                return Result.Failure(new Error("Creating.Failed", "An error occurred while Creating the driver.", StatusCodes.Status500InternalServerError));
            }
        }
    }
}
