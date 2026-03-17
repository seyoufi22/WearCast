namespace WearCast.Api.Features.DesignedProductManagement.FactoryProductImages.AddFactoryProductImage
{
    public class AddFactoryProductImageRequestValidator : AbstractValidator<AddFactoryProductImageRequest>
    {
        public AddFactoryProductImageRequestValidator()
        {
            RuleFor(x => x.ColorId).GreaterThan(0).WithMessage("Invalid Color Id.");

            RuleFor(x => x.ViewSide).IsInEnum().WithMessage("Invalid view side.");

            RuleFor(x => x.Image)
                .NotNull()
                .IsValidImage();
        }
    }
}
