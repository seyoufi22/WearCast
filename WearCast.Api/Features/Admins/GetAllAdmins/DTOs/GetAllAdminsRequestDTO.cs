using WearCast.Api.Common.Views;

namespace WearCast.Api.Features.Admins.GetAllAdmins.DTOs
{
    public class GetAllAdminsRequestDTO : IRequest<Result<PagingViewModel<GetAllAdminsResponseDTO>>>
    {
        public int PageIndex { get; set; } = 1;
        public int PageSize { get; set; } = 100;

        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public string? Email { get; set; }
        public AdminRole? Role { get; set; }
        public bool? IsDeleted { get; set; }
    }

    public class GetAllAdminsValidator : AbstractValidator<GetAllAdminsRequestDTO>
    {
        public GetAllAdminsValidator()
        {
            RuleFor(x => x.PageIndex)
                .GreaterThan(0)
                .WithMessage("Page index must be greater than 0.");

            RuleFor(x => x.PageSize)
                .InclusiveBetween(1, 100)
                .WithMessage("Page size must be between 1 and 100.");

            RuleFor(x => x.FirstName)
                .MaximumLength(100)
                .WithMessage("First name cannot exceed 100 characters.")
                .When(x => !string.IsNullOrWhiteSpace(x.FirstName));

            RuleFor(x => x.LastName)
                .MaximumLength(100)
                .WithMessage("Last name cannot exceed 100 characters.")
                .When(x => !string.IsNullOrWhiteSpace(x.LastName));

            RuleFor(x => x.Email)
                .EmailAddress()
                .WithMessage("Invalid email format.")
                .When(x => !string.IsNullOrWhiteSpace(x.Email));

             RuleFor(x => x.Role)
                .IsInEnum()
                .WithMessage("Invalid Role Name")
                .When(x => x.Role.HasValue);
        }
    }
}
