namespace WearCast.Api.Features.DesignedProductManagement.FactoryProductColors.AddFactoryProductColor
{
    public class AddFactoryProductColorRequestValidator : AbstractValidator<AddFactoryProductColorRequest>
    {
        public AddFactoryProductColorRequestValidator()
        {
            RuleFor(x => x.ProductSlug)
                .NotEmpty().WithMessage("Product slug is required.");

            RuleFor(x => x.Name)
                .NotEmpty().WithMessage("Color name is required.")
                .MaximumLength(100).WithMessage("Color name is too long.");

            RuleFor(x => x.HexCode)
                .NotEmpty().WithMessage("Hex code is required.")
                .Matches(RegexPatterns.HexColorCode)
                .WithMessage("Invalid Hex Code format. Example: #000000 or #FFF");
        }
    }
}
