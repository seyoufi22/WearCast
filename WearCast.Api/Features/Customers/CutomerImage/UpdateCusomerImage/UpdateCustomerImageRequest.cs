namespace WearCast.Api.Features.Customers.CutomerImage.UpdateCusomerImage
{
    public record UpdateCustomerImageRequest(IFormFile NewImage, int? ProvidedCustomerId = null) : IRequest<Result>;
}
