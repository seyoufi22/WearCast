namespace WearCast.Api.Features.DesignedProductManagement.FactoryProductImages.UpdateFactoryProductImage
{
    public class UpdateFactoryProductImageRequestValidator : AbstractValidator<UpdateFactoryProductImageRequest>
    {
        public UpdateFactoryProductImageRequestValidator()
        {
            RuleFor(x => x.ImageId).GreaterThan(0).WithMessage("Invalid Image Id.");

            RuleFor(x => x.ViewSide).IsInEnum().WithMessage("Invalid view side.");

            RuleFor(x => x.Image)
                .IsValidImage();
        }
    }
}
