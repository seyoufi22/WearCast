namespace WearCast.Api.Features.DesignedProductManagement.CustomerUploadedImages.AddCustomerUploadedImage
{
    public record AddCustomerUploadedImageRequest(
        IFormFile Image
        ) : IRequest<Result<CustomerUploadedImageResponse>>;
}
