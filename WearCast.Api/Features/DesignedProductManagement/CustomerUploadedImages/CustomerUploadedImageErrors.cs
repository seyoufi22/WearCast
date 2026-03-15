namespace WearCast.Api.Features.DesignedProductManagement.CustomerUploadedImages
{
    public static class CustomerUploadedImageErrors
    {
        public static readonly Error ImageNotFound =
            new Error("Image.NotFound", "Image not found or you don't have access to it.", StatusCodes.Status404NotFound);
    }
}
