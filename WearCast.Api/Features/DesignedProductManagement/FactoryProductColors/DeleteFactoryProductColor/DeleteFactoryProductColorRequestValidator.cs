namespace WearCast.Api.Features.DesignedProductManagement.FactoryProductColors.DeleteFactoryProductColor
{
    public class DeleteFactoryProductColorRequestValidator : AbstractValidator<DeleteFactoryProductColorRequest>
    {
        public DeleteFactoryProductColorRequestValidator()
        {
            RuleFor(x => x.ColorId)
                .GreaterThan(0).WithMessage("Invalid Color Id.");

        }
    }
}
