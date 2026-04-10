namespace WearCast.Api.Features.Factories.CreateFactoryManager
{
    public record CreateFactoryManagerRequest(
        string Email,
        string FirstName,
        string LastName,
        string PhoneNumber,
        string Password,
        string ConfirmPassword,

        int FactoryId
       ) : IRequest<Result<CreateFactoryManagerResponse>>;
}
