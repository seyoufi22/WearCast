namespace WearCast.Api.Features.Admins.DeleteAdmin.DTOs
{
    public class DeleteAdminRequestDTO : IRequest<Result>
    {
        public string Id { get; set; } = string.Empty;
    }

    public class DeleteAdminValidator : AbstractValidator<DeleteAdminRequestDTO>
    {
        public DeleteAdminValidator()
        {
            RuleFor(x => x.Id)
                .NotEmpty()
                .WithMessage("Admin ID is required.");
        }
    }
}
