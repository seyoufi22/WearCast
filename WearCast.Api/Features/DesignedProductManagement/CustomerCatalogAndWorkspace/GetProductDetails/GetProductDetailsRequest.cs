namespace WearCast.Api.Features.DesignedProductManagement.CustomerCatalogAndWorkspace.GetProductDetails
{
    public record GetProductDetailsRequest(int ProductId, int? DefaultColorId) : IRequest<Result<GetProductDetailsResponse>>;
}
