using WearCast.Api.Features.AuthenticationManagement.GetUserId.DTOs;

namespace WearCast.Api.Features.AuthenticationManagement.GetUserId.Handlers
{
    public class GetUserIdHandler(UserManager<ApplicationUser> userManager)
        : IRequestHandler<GetUserIdRequestDTO, Result<GetUserIdResponseDTO>>
    {
        public async Task<Result<GetUserIdResponseDTO>> Handle(GetUserIdRequestDTO request, CancellationToken cancellationToken)
        {
            if (await userManager.FindByEmailAsync(request.Email) is not { } user)
                return Result.Failure<GetUserIdResponseDTO>(UserErrors.InvalidCredentials);

            if (user.IsDisabled)
                return Result.Failure<GetUserIdResponseDTO>(UserErrors.DisabledUser);

            bool isPasswordValid = await userManager.CheckPasswordAsync(user, request.Password);

            if (isPasswordValid)
            {
                return Result.Success(new GetUserIdResponseDTO
                {
                    UserId = user.Id
                });
            }

            return Result.Failure<GetUserIdResponseDTO>(UserErrors.InvalidCredentials);
        }
    }
}
