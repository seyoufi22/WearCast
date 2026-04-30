namespace WearCast.Api.Features.Sellers.DeleteSeller;

public class DeleteSellerHandler(
    ApplicationDbContext context,
    EmailHelper emailHelper
) : IRequestHandler<DeleteSellerRequest, Result>
{
    private readonly ApplicationDbContext _context = context;
    private readonly EmailHelper _emailHelper = emailHelper;

    public async Task<Result> Handle(DeleteSellerRequest request, CancellationToken cancellationToken)
    {
        var seller = await _context.Sellers
            .Include(s => s.Managers)
                .ThenInclude(m => m.ApplicationUser)
            .FirstOrDefaultAsync(s => s.Id == request.SellerId, cancellationToken);

        if (seller is null)
            return Result.Failure(SellerErrors.SellerNotFound);

        seller.IsDeleted = true;

        foreach (var manager in seller.Managers)
            await _emailHelper.SendAccountDeletedEmail(
                manager.ApplicationUser!.Email!,
                $"{manager.ApplicationUser.FirstName} {manager.ApplicationUser.LastName}",
                request.Reason
            );

        foreach (var manager in seller.Managers)
        {
            manager.IsDeleted = true;
            manager.ApplicationUser!.IsDeleted = true;
        }

        await _context.SaveChangesAsync(cancellationToken);


        return Result.Success();
    }
}