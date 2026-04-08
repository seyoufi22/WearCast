namespace WearCast.Api.Features.Factories.FactoryManagers.CreateFactoryManager
{
    public record CreateFactoryManagerRequest(
        string Email,
        string FirstName,
        string LastName,
        string PhoneNumber,
        string Password,
        string ConfirmPassword,

        int? ProvidedFactoryId = null
       ) : IRequest<Result<CreateFactoryManagerResponse>>;
}
