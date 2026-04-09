namespace WearCast.Api.Features.DesignedProductManagement.FactoryProductColors.UpdateFactoryProductColorMainImage
{
    public class UpdateFactoryProductColorMainImageRequestValidator : AbstractValidator<UpdateFactoryProductColorMainImageRequest>
    {
        public UpdateFactoryProductColorMainImageRequestValidator()
        {
            RuleFor(x => x.ColorId)
                 .GreaterThan(0).WithMessage("Invalid Color Id.");

            RuleFor(x => x.ProductId)
                .GreaterThan(0).WithMessage("Invalid Product Id.");

            RuleFor(x => x.Image)
                .NotNull()
                .IsValidImage();
        }
    }
}
