namespace WearCast.Api.Features.DesignedProductManagement.CustomerCatalogAndWorkspace.GetProductDetails
{
    public record GetProductDetailsRequest(int Id) : IRequest<Result<GetProductDetailsResponse>>;
}
