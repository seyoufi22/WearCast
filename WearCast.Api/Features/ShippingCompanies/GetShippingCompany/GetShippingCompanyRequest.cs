namespace WearCast.Api.Features.ShippingCompanies.GetShippingCompany;

public class GetShippingCompanyRequest : IRequest<Result<GetShippingCompanyResponse>>
{
    public int? ProvidedCompanyId { get; set; }
}