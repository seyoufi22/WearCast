namespace WearCast.Api.Features.ShippingCompanies.CreateShippingCompanyManager
{
    public record CreateShippingCompanyManagerRequest(
        string Email,
        string FirstName,
        string LastName,
        string PhoneNumber,
        string Password,
        string ConfirmPassword,

        int ShippingCompanyId
        ) : IRequest<Result<CreateShippingCompanyManagerResponse>>;
}
