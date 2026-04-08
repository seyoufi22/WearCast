namespace WearCast.Api.Features.Customers.CutomerImage.DeleteCustomerImage
{
    public record DeleteCustomerImageRequest(int? ProvidedCustomerId = null) : IRequest<Result>;
}
