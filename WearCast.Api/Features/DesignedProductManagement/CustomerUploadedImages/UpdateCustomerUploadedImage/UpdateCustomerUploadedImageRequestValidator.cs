namespace WearCast.Api.Features.DesignedProductManagement.CustomerUploadedImages.UpdateCustomerUploadedImage
{
    public class UpdateCustomerUploadedImageRequestValidator : AbstractValidator<UpdateCustomerUploadedImageRequest>
    {
        public UpdateCustomerUploadedImageRequestValidator()
        {
            RuleFor(x => x.Id)
                .GreaterThan(0)
                .WithMessage("Invalid Image Id.");

            RuleFor(x => x.Image)
                .NotNull()
                .IsValidImage();
        }
    }
}
