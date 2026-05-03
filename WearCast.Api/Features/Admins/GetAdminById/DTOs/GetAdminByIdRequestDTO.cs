namespace WearCast.Api.Features.Admins.GetAdminById.DTOs;

public class GetAdminByIdRequestDTO : IRequest<Result<GetAdminByIdResponseDTO>>
{
    public string Id { get; set; } = string.Empty;
}



public class GetAdminByIdValidator : AbstractValidator<GetAdminByIdRequestDTO>
{
    public GetAdminByIdValidator()
    {
        RuleFor(x => x.Id).NotEmpty().WithMessage("Admin ID is required.");
    }
}