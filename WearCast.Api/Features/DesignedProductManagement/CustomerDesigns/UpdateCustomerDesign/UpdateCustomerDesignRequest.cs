namespace WearCast.Api.Features.DesignedProductManagement.CustomerDesigns.UpdateCustomerDesign
{
    public record UpdateCustomerDesignRequest(
        int Id,
        string ViewDesignsJson,
        int NewProductColorId //new colorId to change the color
        ) : IRequest<Result<CustomerDesignResponse>>;
}
