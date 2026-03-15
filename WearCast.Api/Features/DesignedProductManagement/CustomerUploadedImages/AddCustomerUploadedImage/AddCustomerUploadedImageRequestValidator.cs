namespace WearCast.Api.Features.DesignedProductManagement.CustomerUploadedImages.AddCustomerUploadedImage
{
    public class AddCustomerUploadedImageRequestValidator : AbstractValidator<AddCustomerUploadedImageRequest>
    {
        public AddCustomerUploadedImageRequestValidator()
        {
            RuleFor(x => x.Image)
                .NotNull()
                .IsValidImage();
        }
    }
}
