namespace WearCast.Api.Features.Factories.DeleteFactory;

public record DeleteFactoryRequest(
    int FactoryId,
    string Reason
) : IRequest<Result>;

public record DeleteFactoryBody(
    string Reason
);