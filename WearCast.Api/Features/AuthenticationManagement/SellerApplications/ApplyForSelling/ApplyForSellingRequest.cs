namespace WearCast.Api.Features.AuthenticationManagement.SellerApplications.ApplyForSelling
{
    public record ApplyForSellingRequest(
        string Email,
        string FirstName,
        string LastName,
        string PhoneNumber,
        string Password,
        string ConfirmPassword,

        string SellerName,
        string CommercialRegisterNumber,
        string TaxIdNumber,
        string Description,
        IFormFile Logo,

        string Country,
        string State,
        string City,
        string Street,
        string BuildingNumber
        ) : IRequest<Result>;
}
