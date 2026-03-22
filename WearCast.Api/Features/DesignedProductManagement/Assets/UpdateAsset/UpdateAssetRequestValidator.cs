namespace WearCast.Api.Features.DesignedProductManagement.Assets.UpdateAsset
{
    public class UpdateAssetRequestValidator : AbstractValidator<UpdateAssetRequest>
    {
        public UpdateAssetRequestValidator()
        {
            RuleFor(x => x.Id)
                .GreaterThan(0).WithMessage("Asset ID is required.");

            RuleFor(x => x.Name)
               .NotEmpty().WithMessage("Asset name is required.")
               .MaximumLength(150).WithMessage("Asset name is too long.");

            RuleFor(x => x.Image)
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
