namespace WearCast.Api.Features.DesignedProductManagement.FactoryProductSizes
{
    public static class SizeErrors
    {
        public static readonly Error SizeAlreadyExists = new(
            "DesignedProductSize.AlreadyExists",
            "This size already exists for this product.",
            StatusCodes.Status400BadRequest
        );

        public static readonly Error SizeNotFound = new(
            "DesignedProductSize.NotFound",
            "The specified size was not found for this product.",
            StatusCodes.Status404NotFound
        );
    }
}
