namespace WearCast.Api.Features.DesignedProductManagement.FactoryProductSizes.AddFactoryProductSize
{
    public class AddFactoryProductSizeRequestValidator : AbstractValidator<AddFactoryProductSizeRequest>
    {
        public AddFactoryProductSizeRequestValidator()
        {
            RuleFor(x => x.ProductSlug).NotEmpty().WithMessage("Product slug is required.");
            RuleFor(x => x.Size).IsInEnum().WithMessage("Invalid size selected.");

            RuleFor(x => x.A).GreaterThan(0).When(x => x.A.HasValue);
            RuleFor(x => x.B).GreaterThan(0).When(x => x.B.HasValue);
            RuleFor(x => x.C).GreaterThan(0).When(x => x.C.HasValue);
        }
    }
}
