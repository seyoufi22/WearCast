namespace WearCast.Api.Features.DesignedProductManagement.FactoryProductColors.UpdateFactoryProductColor
{
    public class UpdateFactoryProductColorRequestValidator : AbstractValidator<UpdateFactoryProductColorRequest>
    {
        public UpdateFactoryProductColorRequestValidator()
        {
            RuleFor(x => x.ProductSlug).NotEmpty().WithMessage("Product slug is required.");
            RuleFor(x => x.CurrentColorSlug).NotEmpty().WithMessage("Color slug is required.");

            RuleFor(x => x.Name)
                .NotEmpty().WithMessage("Color name is required.")
                .MaximumLength(100).WithMessage("Color name is too long.");

            RuleFor(x => x.HexCode)
                .NotEmpty().WithMessage("Hex code is required.")
                .Matches(RegexPatterns.HexColorCode).WithMessage("Invalid Hex Code format. Example: #000000 or #FFF");
        }
    }
}
