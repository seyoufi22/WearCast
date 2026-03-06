namespace WearCast.Api.Features.Factories.CreateFactory
{
    public record CreateFactoryRequest(
        string ManagerEmail,
        string ManagerFirstName,
        string ManagerLastName,
        string ManagerPhoneNumber,
        string ManagerPassword,
        string ManagerConfirmPassword,

        string FactoryName,
        string FactoryEmail,
        string FactoryPhoneNumber,
        string FactoryCommercialRegisterNumber,
        string FactoryTaxIdNumber,
        string FactoryDescription,
        IFormFile FactoryLogo,

        string FactoryState,
        string FactoryCity,
        string FactoryStreet,
        string FactoryBuildingNumber

        ) : IRequest<Result>;
}
