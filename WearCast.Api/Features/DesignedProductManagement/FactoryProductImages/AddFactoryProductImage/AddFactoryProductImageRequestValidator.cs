namespace WearCast.Api.Features.DesignedProductManagement.FactoryProductImages.AddFactoryProductImage
{
    public class AddFactoryProductImageRequestValidator : AbstractValidator<AddFactoryProductImageRequest>
    {
        public AddFactoryProductImageRequestValidator()
        {
            RuleFor(x => x.ColorSlug).NotEmpty().WithMessage("Color slug is required.");

            RuleFor(x => x.ViewSide).IsInEnum().WithMessage("Invalid view side.");

            RuleFor(x => x.Image)
                .NotNull()
                .IsValidImage();
        }
    }
}
