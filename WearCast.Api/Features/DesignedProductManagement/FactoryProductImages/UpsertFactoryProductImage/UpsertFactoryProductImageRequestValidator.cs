namespace WearCast.Api.Features.DesignedProductManagement.FactoryProductImages.UpsertFactoryProductImage
{
    public class UpsertFactoryProductImageRequestValidator : AbstractValidator<UpsertFactoryProductImageRequest>
    {
        public UpsertFactoryProductImageRequestValidator()
        {
            RuleFor(x => x.ColorId)
                .GreaterThan(0).WithMessage("Invalid Color Id.");

            RuleFor(x => x.ViewSide)
                .IsInEnum().WithMessage("Invalid View Side. Allowed values are Front, Back, Right, Left.");

            RuleFor(x => x.NewImage)
                .NotNull()
                .IsValidImage();
        }
    }
}
