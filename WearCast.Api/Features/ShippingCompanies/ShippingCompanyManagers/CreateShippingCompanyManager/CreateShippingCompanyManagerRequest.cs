namespace WearCast.Api.Features.ShippingCompanies.ShippingCompanyManagers.CreateShippingCompanyManager
{
    public record CreateShippingCompanyManagerRequest(
        string Email,
        string FirstName,
        string LastName,
        string PhoneNumber,
        string Password,
        string ConfirmPassword,

        int? ProvidedShippingCompanyId = null
        ) : IRequest<Result<CreateShippingCompanyManagerResponse>>;
}
