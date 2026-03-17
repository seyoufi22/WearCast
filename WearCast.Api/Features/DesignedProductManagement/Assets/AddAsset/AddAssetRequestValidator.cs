namespace WearCast.Api.Features.DesignedProductManagement.Assets.AddAsset
{
    public class AddAssetRequestValidator : AbstractValidator<AddAssetRequest>
    {
        public AddAssetRequestValidator()
        {
            RuleFor(x => x.Name)
                .NotEmpty().WithMessage("Asset name is required.")
                .MaximumLength(150).WithMessage("Asset name is too long.");

            RuleFor(x => x.Image)
                .NotNull()
                .IsValidImage();

            RuleFor(x => x.WidthPx)
                .GreaterThan(0).WithMessage("Width must be greater than zero.");

            RuleFor(x => x.HeightPx)
                .GreaterThan(0).WithMessage("Height must be greater than zero.");

            RuleFor(x => x.CategoryId)
                .GreaterThan(0).WithMessage("Category ID is required.");
        }
    }
}
