namespace WearCast.Api.Features.DesignedProductManagement.FactoryProductImages.DeleteFactoryProductImage
{
    public class DeleteFactoryProductImageRequestValidator : AbstractValidator<DeleteFactoryProductImageRequest>
    {
        public DeleteFactoryProductImageRequestValidator()
        {
            RuleFor(x => x.ImageId).GreaterThan(0).WithMessage("Invalid Image Id.");
        }
    }
}
