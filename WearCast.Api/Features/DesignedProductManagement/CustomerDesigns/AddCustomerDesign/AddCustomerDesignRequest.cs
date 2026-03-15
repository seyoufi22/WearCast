namespace WearCast.Api.Features.DesignedProductManagement.CustomerDesigns.AddCustomerDesign
{
    public record AddCustomerDesignRequest(
        string ViewDesignsJson,
        int ProductId,
        int ProductColorId
        ) : IRequest<Result<CustomerDesignResponse>>;
}
