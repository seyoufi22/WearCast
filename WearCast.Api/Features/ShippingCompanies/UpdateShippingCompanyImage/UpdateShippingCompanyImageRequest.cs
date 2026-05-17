namespace WearCast.Api.Features.ShippingCompanies.UpdateShippingCompanyImage
{
    public record UpdateShippingCompanyImageRequest(IFormFile NewLogo) : IRequest<Result>;
}
