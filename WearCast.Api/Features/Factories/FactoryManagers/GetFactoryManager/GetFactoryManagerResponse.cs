namespace WearCast.Api.Features.Factories.FactoryManagers.GetFactoryManager;

public record GetFactoryManagerResponse(
    int Id,
    string FirstName,
    string LastName,
    string? PhoneNumber,
    string? Email
);