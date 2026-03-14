namespace WearCast.Api.Features.DesignedProductManagement
{
    public static class DesignedProductErrors
    {
        public static readonly Error ProductNotFound
            = new("ProductNotFound", "Product does not exist.", StatusCodes.Status404NotFound);

        public static readonly Error ProductAlreadyDeleted = new(
            "DesignedProduct.AlreadyDeleted",
            "This product has already been deleted.",
            StatusCodes.Status400BadRequest
        );

        public static readonly Error FactoryRequiredForAdmin = new(
            "DesignedProduct.FactoryRequired",
            "A valid Factory ID is required when a product is added by an Admin.",
            StatusCodes.Status400BadRequest);

        public static readonly Error FactoryNotFound = new(
            "DesignedProduct.FactoryNotFound",
            "The specified factory does not exist in the system.",
            StatusCodes.Status404NotFound);

        public static readonly Error NoAssociatedFactory = new(
            "Auth.NoAssociatedFactory",
            "Your account is not associated with any factory.",
            StatusCodes.Status403Forbidden);

    }
}
