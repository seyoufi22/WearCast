namespace WearCast.Api.Features.DesignedProductManagement.CustomerDesigns.GetCustomerDesign
{
    public record GetCustomerDesignResponse(
        int Id,
        int DesignedProductId,
        int DesignedProductColorId,
        string ViewDesignsJson,
        string? FrontImageUrl,
        string? BackImageUrl,
        string? RightImageUrl,
        string? LeftImageUrl,
        int AssetCount,
        decimal TotalPrice
     );
}
