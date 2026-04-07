namespace WearCast.Api.Features.Factories.FactoryManagers.UpdateFactoryManager
{
    public record UpdateFactoryManagerRequest(
        string FirstName,
        string LastName,
        string PhoneNumber,
        int? ProvidedManagerId = null
        ) : IRequest<Result>;
}
