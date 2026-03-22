namespace WearCast.Api.Features.DesignedProductManagement.FactoryProductColors.UpdateFactoryProductColor
{
    public class UpdateFactoryProductColorRequestValidator : AbstractValidator<UpdateFactoryProductColorRequest>
    {
        public UpdateFactoryProductColorRequestValidator()
        {
            RuleFor(x => x.ColorId)
                 .GreaterThan(0).WithMessage("Invalid Color Id.");

            RuleFor(x => x.Name)
                .NotEmpty().WithMessage("Color name is required.")
                .MaximumLength(100).WithMessage("Color name is too long.");

            RuleFor(x => x.HexCode)
                .NotEmpty().WithMessage("Hex code is required.")
                .Matches(RegexPatterns.HexColorCode).WithMessage("Invalid Hex Code format. Example: #000000 or #FFF");

            RuleFor(x => x.ProductId)
                .GreaterThan(0).WithMessage("Invalid Product Id.");
        }
    }
}
