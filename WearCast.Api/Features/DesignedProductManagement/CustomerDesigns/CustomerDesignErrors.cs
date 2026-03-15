namespace WearCast.Api.Features.DesignedProductManagement.CustomerDesigns
{
    public static class CustomerDesignErrors
    {
        public static readonly Error DesignNotFound =
            new Error("Design.NotFound", "The design was not found or you don't have access to it.", StatusCodes.Status404NotFound);
    }
}
