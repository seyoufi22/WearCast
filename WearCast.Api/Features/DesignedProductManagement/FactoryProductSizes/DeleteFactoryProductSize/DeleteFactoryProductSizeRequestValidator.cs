namespace WearCast.Api.Features.DesignedProductManagement.FactoryProductSizes.DeleteFactoryProductSize
{
    public class DeleteFactoryProductSizeRequestValidator : AbstractValidator<DeleteFactoryProductSizeRequest>
    {
        public DeleteFactoryProductSizeRequestValidator()
        {
            RuleFor(x => x.ProductSlug).NotEmpty().WithMessage("Product slug is required.");
            RuleFor(x => x.Size).IsInEnum().WithMessage("Invalid size selected.");
        }
    }
}
