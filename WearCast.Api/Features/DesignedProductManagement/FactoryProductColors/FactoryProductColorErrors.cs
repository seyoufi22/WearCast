namespace WearCast.Api.Features.DesignedProductManagement.FactoryProductColors
{
    public static class FactoryProductColorErrors
    {
        public static readonly Error ColorNotFound
            = new("ColorNotFound", "Color does not exist.", StatusCodes.Status404NotFound);
    }
}
