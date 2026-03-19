namespace WearCast.Api.Features.FixedProductColor.CreateFixedProductColor.DTOs;

public record CreateFixedProductColorCommandDto(
    CreateFixedProductColorRequestDto request,
    int sellerId
) : IRequest<Result>;


public class CreateFixedProductColorValidator : AbstractValidator<CreateFixedProductColorCommandDto>
{
    public CreateFixedProductColorValidator()
    {

        RuleFor(x => x.request.ProductId)
            .GreaterThan(0);

        RuleFor(x => x.request.ColorName.Trim()).NotEmpty()
            .MaximumLength(50).WithMessage("Color name cannot exceed 50 characters.")
            .Must(name => !string.IsNullOrWhiteSpace(name))
            .WithMessage("Color name cannot be empty spaces.");

        RuleFor(x => x.request.ColorCode.Trim())
            .NotEmpty().WithMessage("Color code is required.")
            .Matches("^#([A-Fa-f0-9]{6}|[A-Fa-f0-9]{3})$")
            .WithMessage("Invalid color code format. It should be a valid Hex code (e.g., #FFFFFF or #FFF).")
            .MaximumLength(20).WithMessage("Color code is too long.");

        RuleFor(x => x.request.Image)
            .NotNull().WithMessage("Image is required.");


        RuleFor(x => x.request.Sizes)
            .NotEmpty().WithMessage("Sizes are required.")
            .Must(sizes => sizes.All(s => s is not null))
            .WithMessage("Size entries cannot be empty.")
            .Must(sizes => sizes.Select(s => s.Size).Distinct().Count() == sizes.Count)
            .WithMessage("Sizes must be unique. You cannot add the same size multiple times.");

        RuleForEach(x => x.request.Sizes)
            .Where(s => s is not null) 
            .ChildRules(size =>
            {
                size.RuleFor(s => s.Size)
                    .IsInEnum()
                    .WithMessage("One or more selected sizes are invalid.");

                size.RuleFor(s => s.Quantity)
                    .GreaterThan(0) 
                    .WithMessage("Quantity must be greater than 0.");
            });
    }
}