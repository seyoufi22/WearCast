namespace WearCast.Api.Features.DesignedProductManagement.FactoryProductColors
{
    public static class FactoryProductColorErrors
    {
        public static readonly Error ColorNotFound
            = new("ColorNotFound", "Color does not exist.", StatusCodes.Status404NotFound);

        public static readonly Error ColorAlreadyExists = new(
                "DesignedProduct.ColorAlreadyExists",
                "A color with this hex code already exists for the specified product.",
                StatusCodes.Status409Conflict);
    }
}
