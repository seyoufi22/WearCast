namespace WearCast.Api.Features.ShippingCompanies.DeleteShippingCompany;

public record DeleteShippingCompanyRequest(
    int CompanyId,
    string Reason
) : IRequest<Result>;

public record DeleteShippingCompanyBody(
    string Reason
);