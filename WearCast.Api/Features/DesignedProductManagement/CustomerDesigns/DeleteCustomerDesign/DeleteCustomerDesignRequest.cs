namespace WearCast.Api.Features.DesignedProductManagement.CustomerDesigns.DeleteCustomerDesign
{
    public record DeleteCustomerDesignRequest(int Id) : IRequest<Result>;
}
