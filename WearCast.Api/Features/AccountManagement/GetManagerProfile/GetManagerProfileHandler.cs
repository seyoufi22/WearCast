namespace WearCast.Api.Features.AccountManagement.GetManagerProfile;

using WearCast.Api.Abstractions;
public class GetManagerProfileHandler : IRequestHandler<GetManagerProfileRequest, Result<GetManagerProfileResponse>>
{
    private readonly ApplicationDbContext _context ;
    public GetManagerProfileHandler(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<Result<GetManagerProfileResponse>> Handle(GetManagerProfileRequest request, CancellationToken cancellationToken)
    {
        var manager = await _context.Users
            .Where(u => u.Id == request.Id)
            .Select(u => new GetManagerProfileResponse(
                u.FirstName,
                u.LastName,
                u.PhoneNumber,
                u.Email
            ))
            .FirstOrDefaultAsync(cancellationToken);
        if (manager is null)
        {
            return Result.Failure<GetManagerProfileResponse>(
                new Error("Manager.NotFound", $"Manager with ID {request.Id} was not found.", 404)
            );
        }

        return Result.Success(manager);
    }

}
