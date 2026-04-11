namespace WearCast.Api.Features.Factories.FactoryManagers.GetFactoryManager;

public record GetFactoryManagerRequest(
    int? ProvidedManagerId = null
) : IRequest<Result<GetFactoryManagerResponse>>;