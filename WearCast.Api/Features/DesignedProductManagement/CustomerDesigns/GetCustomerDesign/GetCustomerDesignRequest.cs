namespace WearCast.Api.Features.DesignedProductManagement.CustomerDesigns.GetCustomerDesign
{
    public record GetCustomerDesignRequest(int Id) : IRequest<Result<GetCustomerDesignResponse>>;
}
