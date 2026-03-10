namespace WearCast.Api.Features.DesignedProductManagement.FactoryProductImages.DeleteFactoryProductImage
{
    public class DeleteFactoryProductImageRequestValidator : AbstractValidator<DeleteFactoryProductImageRequest>
    {
        public DeleteFactoryProductImageRequestValidator()
        {
            RuleFor(x => x.ColorSlug).NotEmpty().WithMessage("Color slug is required.");
            RuleFor(x => x.ImageId).GreaterThan(0).WithMessage("Image ID is required.");
        }
    }
}
