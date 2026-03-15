namespace WearCast.Api.Features.DesignedProductManagement.CustomerUploadedImages.DeleteCustomerUploadedImages
{
    public record DeleteCustomerUploadedImagesRequest(int Id) : IRequest<Result>;
}
