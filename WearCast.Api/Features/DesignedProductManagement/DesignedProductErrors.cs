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
    }
}
