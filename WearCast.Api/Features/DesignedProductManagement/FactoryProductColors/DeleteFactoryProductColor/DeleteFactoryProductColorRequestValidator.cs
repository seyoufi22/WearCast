namespace WearCast.Api.Features.DesignedProductManagement.FactoryProductColors.DeleteFactoryProductColor
{
    public class DeleteFactoryProductColorRequestValidator : AbstractValidator<DeleteFactoryProductColorRequest>
    {
        public DeleteFactoryProductColorRequestValidator()
        {
            RuleFor(x => x.ProductSlug).NotEmpty().WithMessage("Product slug is required.");
            RuleFor(x => x.CurrentColorSlug).NotEmpty().WithMessage("Color slug is required.");
        }
    }
}
