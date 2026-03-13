using WearCast.Api.Features.DesignedProductManagement.CustomerUploadedImages.AddCustomerUploadedImage;

namespace WearCast.Api.Features.DesignedProductManagement.CustomerUploadedImages.UpdateCustomerUploadedImage
{
    public record UpdateCustomerUploadedImageRequest(
        int Id,
        IFormFile Image
        ) : IRequest<Result<CustomerUploadedImageResponse>>;
}
