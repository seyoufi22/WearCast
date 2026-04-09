namespace WearCast.Api.Features.DesignedProductManagement.FactoryProducts.CreateDesignedProduct
{
    public record CreateDesignedProductRequest(
        string Name,
        string Description,
        List<TargetAudience> TargetAudiences,
        DressStyle DressStyle,
        decimal Price,
        int CanvasWidth,
        int CanvasHeight,
        int CategoryId,
        int? FactoryId
        ) : IRequest<Result<CreateDesignedProductResponse>>
    {
        public List<CreateProductSizeRequest> SizeDetails { get; init; } = [];
    };

    public record CreateProductSizeRequest(
        Size Size,
        decimal? A,
        decimal? B,
        decimal? C
    );
}
