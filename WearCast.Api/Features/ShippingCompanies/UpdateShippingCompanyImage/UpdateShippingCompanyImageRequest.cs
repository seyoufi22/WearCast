namespace WearCast.Api.Features.ShippingCompanies.UpdateShippingCompanyImage
{
    public record UpdateShippingCompanyImageRequest(IFormFile NewLogo, int? ProvidedShippingCompanyId = null) : IRequest<Result>;
}
