namespace WearCast.Api.Features.DesignedProductManagement.FactoryProducts.UpdateDesignedProduct
{
    public record UpdateDesignedProductRequest(
        string CurrentSlug,
        string Name,
        string Description,
        TargetAudience TargetAudience,
        decimal Price,
        int CanvasWidth,
        int CanvasHeight,
        int CategoryId
        ) : IRequest<Result>;
}
