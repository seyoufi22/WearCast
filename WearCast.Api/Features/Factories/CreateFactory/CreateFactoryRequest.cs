namespace WearCast.Api.Features.Factories.CreateFactory
{
    public record CreateFactoryRequest(
        string FactoryManagerEmail,
        string FactoryManagerFirstName,
        string FactoryManagerLastName,
        string FactoryManagerPhoneNumber,
        string FactoryManagerPassword,
        string FactoryManagerConfirmPassword,

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
