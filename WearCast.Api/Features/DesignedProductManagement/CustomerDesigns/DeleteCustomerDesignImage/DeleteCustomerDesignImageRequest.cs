namespace WearCast.Api.Features.DesignedProductManagement.CustomerDesigns.DeleteCustomerDesignImage
{
    public record DeleteCustomerDesignImageRequest(int Id, ViewSide Side) : IRequest<Result>;
}
