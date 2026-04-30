namespace WearCast.Api.Features.Factories.DeleteFactory;

public class DeleteFactoryHandler(
    ApplicationDbContext context,
    EmailHelper emailHelper
) : IRequestHandler<DeleteFactoryRequest, Result>
{
    private readonly ApplicationDbContext _context = context;
    private readonly EmailHelper _emailHelper = emailHelper;

    public async Task<Result> Handle(DeleteFactoryRequest request, CancellationToken cancellationToken)
    {
        var factory = await _context.Factories
            .Include(f => f.Managers)
                .ThenInclude(m => m.ApplicationUser)
            .FirstOrDefaultAsync(x => x.Id == request.FactoryId && !x.IsDeleted, cancellationToken);

        if (factory is null)
            return Result.Failure(FactoryErrors.FactoryNotFound);

        factory.IsDeleted = true;

        foreach (var manager in factory.Managers)
        {
            manager.IsDeleted = true;
            manager.ApplicationUser!.IsDeleted = true;
        }

        await _context.SaveChangesAsync(cancellationToken);

        foreach (var manager in factory.Managers)
            await _emailHelper.SendAccountDeletedEmail(
                manager.ApplicationUser!.Email!,
                $"{manager.ApplicationUser.FirstName} {manager.ApplicationUser.LastName}",
                request.Reason
            );

        return Result.Success();
    }
}