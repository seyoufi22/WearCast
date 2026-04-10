namespace WearCast.Api.Features.DesignedProductManagement.CustomerCatalogAndWorkspace.GetProductDetails
{
    public record GetProductDetailsResponse(
        int Id,
        string Name,
        string Description,
        List<string> TargetAudience,
        decimal Price,

        int CanvasWidth,
        int CanvasHeight,

        List<SizeDetailsResponse> SizeDetails,
        List<ColorVariantResponse> Colors
    );

    public record SizeDetailsResponse(
        string Size,
        decimal? A,
        decimal? B,
        decimal? C
    );

    public record ColorVariantResponse(
        int Id,
        string Name,
        string HexCode,
        string MainImageUrl,
        List<ImageResponse> Images
    );

    public record ImageResponse(
        string ImageUrl,
        string ViewSide
    );
}
