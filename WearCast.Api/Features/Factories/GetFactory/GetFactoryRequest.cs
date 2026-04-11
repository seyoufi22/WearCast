namespace WearCast.Api.Features.Factories.GetFactory;

public record GetFactoryRequest(
    int? ProvidedFactoryId = null
) : IRequest<Result<GetFactoryResponse>>;