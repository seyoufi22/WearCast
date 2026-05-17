namespace WearCast.Api.Features.AuthenticationManagement.GetUserId.DTOs
{
    public class GetUserIdRequestDTO : IRequest<Result<GetUserIdResponseDTO>>
    {
        public string Email { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
    }

    public class GetUserIdResponseDTO
    {
        public string UserId { get; set; } = string.Empty;
    }

    public class GetUserIdValidator : AbstractValidator<GetUserIdRequestDTO>
    {
        public GetUserIdValidator()
        {
            RuleFor(x => x.Email)
                .NotEmpty().WithMessage("Email is required.")
                .EmailAddress().WithMessage("Invalid email format.");

            RuleFor(x => x.Password)
                .NotEmpty().WithMessage("Password is required.");
        }
    }
}
