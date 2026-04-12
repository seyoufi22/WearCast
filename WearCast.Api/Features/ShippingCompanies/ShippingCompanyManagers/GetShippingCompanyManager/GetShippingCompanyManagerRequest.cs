namespace WearCast.Api.Features.ShippingCompanies.ShippingCompanyManagers.GetShippingCompanyManager;

public class GetShippingCompanyManagerRequest : IRequest<Result<GetShippingCompanyManagerResponse>>
{
    public int? ProvidedManagerId { get; set; }
}