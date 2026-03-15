namespace WearCast.Api.Features.DesignedProductManagement.CustomerUploadedImages.DeleteCustomerUploadedImages
{
    public class DeleteCustomerUploadedImagesRequestValidator : AbstractValidator<DeleteCustomerUploadedImagesRequest>
    {
        public DeleteCustomerUploadedImagesRequestValidator()
        {
            RuleFor(x => x.Id).GreaterThan(0).WithMessage("Invalid Image Id.");
        }
    }
}
