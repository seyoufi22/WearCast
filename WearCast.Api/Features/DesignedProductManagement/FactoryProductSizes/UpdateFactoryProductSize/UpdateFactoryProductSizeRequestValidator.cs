namespace WearCast.Api.Features.DesignedProductManagement.FactoryProductSizes.UpdateFactoryProductSize
{
    public class UpdateFactoryProductSizeRequestValidator : AbstractValidator<UpdateFactoryProductSizeRequest>
    {
        public UpdateFactoryProductSizeRequestValidator()
        {
            RuleFor(x => x.ProductSlug).NotEmpty().WithMessage("Product slug is required.");
            RuleFor(x => x.Size).IsInEnum().WithMessage("Invalid size selected.");

            RuleFor(x => x.A).GreaterThan(0).When(x => x.A.HasValue);
            RuleFor(x => x.B).GreaterThan(0).When(x => x.B.HasValue);
            RuleFor(x => x.C).GreaterThan(0).When(x => x.C.HasValue);
        }
    }
}
