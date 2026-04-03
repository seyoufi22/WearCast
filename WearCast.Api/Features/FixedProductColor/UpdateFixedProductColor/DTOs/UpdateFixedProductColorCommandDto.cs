namespace WearCast.Api.Features.FixedProductColor.UpdateFixedProductColor.DTOs;

public record UpdateFixedProductColorCommandDto(UpdateFixedProductColorRequestDto request,int sellerId, bool isAdminRequest) : IRequest<Result>;
public class UpdateFixedProductColorValidator : AbstractValidator<UpdateFixedProductColorCommandDto>
{

    public UpdateFixedProductColorValidator()
    {
        RuleFor(x => x.request.ColorId).GreaterThan(0);

        RuleFor(x => x.request.ColorName.Trim()).NotEmpty().WithMessage("Color name is required.")
            .MaximumLength(50).WithMessage("Color name cannot exceed 50 characters.")
            .Must(name => !string.IsNullOrWhiteSpace(name))
            .WithMessage("Color name cannot be empty spaces.");

        RuleFor(x => x.request.ColorCode.Trim())
            .NotEmpty()
            .Matches("^#([A-Fa-f0-9]{6}|[A-Fa-f0-9]{3})$")
            .WithMessage("Invalid color code format. It should be a valid Hex code (e.g., #FFFFFF).")
            .MaximumLength(20);
    }
}