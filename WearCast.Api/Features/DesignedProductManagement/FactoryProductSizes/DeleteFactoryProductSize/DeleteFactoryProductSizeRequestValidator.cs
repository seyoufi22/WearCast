namespace WearCast.Api.Features.DesignedProductManagement.FactoryProductSizes.DeleteFactoryProductSize
{
    public class DeleteFactoryProductSizeRequestValidator : AbstractValidator<DeleteFactoryProductSizeRequest>
    {
        public DeleteFactoryProductSizeRequestValidator()
        {
            RuleFor(x => x.Id).GreaterThan(0).WithMessage("Invalid Size Id.");
        }
    }
}
