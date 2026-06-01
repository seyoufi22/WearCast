namespace WearCast.Api.Features.Factories.FactoryManagers.DeleteFactoryManager;

public record DeleteFactoryManagerRequest(
    int FactoryManagerId,
    string CurrentUserId,
    bool IsAdmin,
    string Reason
) : IRequest<Result>;

public record DeleteFactoryManagerBody(
    string Reason
);