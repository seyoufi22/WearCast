using WearCast.Api.Features.Factories.FactoryManagers.GetFactoryManager;

namespace WearCast.Api.Features.Factories.FactoryManagers.GetAllFactoryManagers;

public class GetAllFactoryManagersHandler(
    ApplicationDbContext context
) : IRequestHandler<GetAllFactoryManagersRequest, Result<List<GetFactoryManagerResponse>>>
{
    private readonly ApplicationDbContext _context = context;

    public async Task<Result<List<GetFactoryManagerResponse>>> Handle(GetAllFactoryManagersRequest request, CancellationToken cancellationToken)
    {
        var managers = await _context.Users
            .AsNoTracking()
            .Where(u => u.FactoryManager != null
                     && !u.FactoryManager.IsDeleted)
            .Select(u => new GetFactoryManagerResponse(
                u.FactoryManager!.Id,
                u.FirstName,
                u.LastName,
                u.PhoneNumber
            ))
            .ToListAsync(cancellationToken);

        return Result.Success(managers);
    }
}