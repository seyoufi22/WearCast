using WearCast.Api.Features.Factories.FactoryManagers.GetFactoryManager;

namespace WearCast.Api.Features.Factories.FactoryManagers.GetAllFactoryManagers;

public record GetAllFactoryManagersRequest : IRequest<Result<List<GetFactoryManagerResponse>>>;