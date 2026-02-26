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
            var phoneNumberIsExists = await _context.Users.AnyAsync(x => x.PhoneNumber == request.PhoneNumber, cancellationToken);

            if (phoneNumberIsExists)
                return Result.Failure(UserErrors.DublicatedPhoneNumber);

            var nationalIdIsExists = await _context.Drivers.AnyAsync(x => x.NationalId == request.NationalId, cancellationToken);

            if (nationalIdIsExists)
                return Result.Failure(DriverErrors.DublicatedNationalId);

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

                var driver = new Driver
                {
                    ProfileImageUrl = profileImageUrl,
                    NationalId = request.NationalId,
                    VehicleType = request.VehicleType,
                    VehiclePlateNumber = request.VehiclePlateNumber,
                    UserId = user.Id
                };

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
