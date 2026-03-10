namespace WearCast.Api.Features.DesignedProductManagement.FactoryProductImages.UpdateFactoryProductImage
{
    public class UpdateFactoryProductImageRequestValidator : AbstractValidator<UpdateFactoryProductImageRequest>
    {
        public UpdateFactoryProductImageRequestValidator()
        {
            RuleFor(x => x.ColorSlug).NotEmpty().WithMessage("Color slug is required.");
            RuleFor(x => x.ImageId).GreaterThan(0).WithMessage("Image ID is required.");
            RuleFor(x => x.ViewSide).IsInEnum().WithMessage("Invalid view side.");

            RuleFor(x => x.Image)
                .IsValidImage();
        }
    }
}
