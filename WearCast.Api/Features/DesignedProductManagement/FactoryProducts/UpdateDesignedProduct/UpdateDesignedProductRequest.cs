namespace WearCast.Api.Features.DesignedProductManagement.FactoryProducts.UpdateDesignedProduct
{
    public record UpdateDesignedProductRequest(
        int Id,
        string Name,
        string Description,
        List<TargetAudience> TargetAudiences,
        decimal Price,
        int CanvasWidth,
        int CanvasHeight,
        int CategoryId
        ) : IRequest<Result>;
}
