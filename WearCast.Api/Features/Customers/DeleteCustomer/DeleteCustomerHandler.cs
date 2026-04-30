namespace WearCast.Api.Features.Customers.DeleteCustomer;

public class DeleteCustomerHandler(
    ApplicationDbContext context,
    EmailHelper emailHelper
) : IRequestHandler<DeleteCustomerRequest, Result>
{
    private readonly ApplicationDbContext _context = context;
    private readonly EmailHelper _emailHelper = emailHelper;

    public async Task<Result> Handle(DeleteCustomerRequest request, CancellationToken cancellationToken)
    {
        var customer = await _context.Customers
            .Include(c => c.ApplicationUser)
            .FirstOrDefaultAsync(x => x.Id == request.CustomerId, cancellationToken);

        if (customer is null)
            return Result.Failure(CustomerErrors.CustomerNotFound);

        customer.IsDeleted = true;
        customer.ApplicationUser!.IsDeleted = true;

        await _context.SaveChangesAsync(cancellationToken);

        await _emailHelper.SendAccountDeletedEmail(
            customer.ApplicationUser!.Email!,
            $"{customer.ApplicationUser.FirstName} {customer.ApplicationUser.LastName}",
            request.Reason
        );

        return Result.Success();
    }
}