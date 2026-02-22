using System.Security.Cryptography;

using WearCast.Api.Common.Email;
using WearCast.Api.Entities.Identity;

namespace WearCast.Api.Features.AuthenticationManagement.Register
{
    public class RegisterCustomerHandler(
        UserManager<ApplicationUser>userManager,
        ILogger<RegisterCustomerHandler> logger,
        IMapper mapper,
        EmailHelper emailHelper
        ) : IRequestHandler<RegisterCustomerRequest, Result<RegisterCustomerResponse>>
    {
        private readonly UserManager<ApplicationUser> _userManager = userManager;
        private readonly ILogger<RegisterCustomerHandler> _logger = logger;
        private readonly IMapper _mapper = mapper;
        private readonly EmailHelper _emailHelper = emailHelper;

        public async Task<Result<RegisterCustomerResponse>> Handle(RegisterCustomerRequest request, CancellationToken cancellationToken)
        {
            var emailIsExists = await _userManager.Users.AnyAsync(x => x.Email == request.Email, cancellationToken);

            if (emailIsExists)
                return Result.Failure<RegisterCustomerResponse>(UserErrors.DublicatedEmail);

            var phoneNumberIsExists = await _userManager.Users.AnyAsync(x => x.PhoneNumber == request.PhoneNumber, cancellationToken);

            if (phoneNumberIsExists)
                return Result.Failure<RegisterCustomerResponse>(UserErrors.DublicatedPhoneNumber);

            var user = _mapper.Map<ApplicationUser>(request);

            var result = await _userManager.CreateAsync(user, request.Password);

            if (result.Succeeded)
            {
                var code = RandomNumberGenerator.GetInt32(100000, 1000000).ToString();
                user.EmailConfirmationCode = code;
                user.EmailConfirmationCodeExpiration = DateTime.UtcNow.AddMinutes(60);

                await userManager.UpdateAsync(user);

                _logger.LogInformation("Confirmation OTP: {code}", code);

                await _emailHelper.SendConfirmationEmail(user, code);

                return Result.Success(new RegisterCustomerResponse(user.Id));
            }

            var error = result.Errors.First();

            return Result.Failure<RegisterCustomerResponse>(new Error(error.Code, error.Description, StatusCodes.Status400BadRequest));
        }
    }
}
