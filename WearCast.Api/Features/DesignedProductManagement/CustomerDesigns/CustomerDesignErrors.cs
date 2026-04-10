namespace WearCast.Api.Features.DesignedProductManagement.CustomerDesigns
{
    public static class CustomerDesignErrors
    {
        public static readonly Error DesignNotFound =
            new Error("Design.NotFound", "The design was not found or you don't have access to it.", StatusCodes.Status404NotFound);

        public static readonly Error DesignInCart =
            new("CustomerDesign.InCart", "This design cannot be deleted because it's currently in your shopping cart.", StatusCodes.Status400BadRequest);
    }
}
