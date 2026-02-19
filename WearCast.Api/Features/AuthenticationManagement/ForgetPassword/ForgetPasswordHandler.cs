
using Microsoft.AspNetCore.Identity;
using System.Security.Cryptography;
using WearCast.Api.Common.Email;
using WearCast.Api.Entities.Identity;

namespace WearCast.Api.Features.AuthenticationManagement.ForgetPassword
{
    public class ForgetPasswordHandler(
        UserManager<ApplicationUser>userManager,
        EmailHelper emailHelper,
        ILogger<ForgetPasswordHandler> logger
        ) : IRequestHandler<ForgetPasswordRequest, Result<ForgetPasswordResponse>>
    {
        private readonly UserManager<ApplicationUser> _userManager = userManager;
        private readonly EmailHelper _emailHelper = emailHelper;
        private readonly ILogger<ForgetPasswordHandler> _logger = logger;

        public async Task<Result<ForgetPasswordResponse>> Handle(ForgetPasswordRequest request, CancellationToken cancellationToken)
        {
            if (await _userManager.FindByEmailAsync(request.Email) is not { } user)
                return Result.Success<ForgetPasswordResponse>(null!); 

            if (!user.EmailConfirmed)
                return Result.Failure<ForgetPasswordResponse>(UserErrors.EmailNotConfirmed);

            
            var code = RandomNumberGenerator.GetInt32(100000, 1000000).ToString();

            user.ResetPasswordCode = code;
            user.ResetPasswordCodeExpiration = DateTime.UtcNow.AddMinutes(60);

            await _userManager.UpdateAsync(user);

            _logger.LogInformation("Reset Password OTP: {code}", code);

            await _emailHelper.SendResetPasswordEmail(user, code);

            return Result.Success(new ForgetPasswordResponse(user.Email!));
        }
    }
}
