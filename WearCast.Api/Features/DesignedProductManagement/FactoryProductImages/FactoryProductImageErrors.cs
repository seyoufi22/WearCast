namespace WearCast.Api.Features.DesignedProductManagement.FactoryProductImages
{
    public static class FactoryProductImageErrors
    {
        public static readonly Error ImageSideAlreadyExists = new(
            "DesignedProductImage.SideAlreadyExists",
            "An image for this view side already exists for this color.",
            StatusCodes.Status400BadRequest
        );

        public static readonly Error ImageNotFound = new(
            "DesignedProductImage.NotFound",
            "The specified image was not found.",
            StatusCodes.Status404NotFound
        );
    }
}
