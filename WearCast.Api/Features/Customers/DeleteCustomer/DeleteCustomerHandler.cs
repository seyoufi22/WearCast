namespace WearCast.Api.Features.Customers.DeleteCustomer;

using Microsoft.Extensions.Logging;

public class DeleteCustomerHandler(
    ApplicationDbContext context,
    EmailHelper emailHelper,
    ILogger<DeleteCustomerHandler> logger 
) : IRequestHandler<DeleteCustomerRequest, Result>
{
    private readonly ApplicationDbContext _context = context;
    private readonly EmailHelper _emailHelper = emailHelper;
    private readonly ILogger<DeleteCustomerHandler> _logger = logger;

    public async Task<Result> Handle(DeleteCustomerRequest request, CancellationToken cancellationToken)
    {
        var customerData = await _context.Customers
            .Where(c => c.Id == request.CustomerId)
            .Select(c => new
            {
                Customer = c,
                UserInfo = new
                {
                    Email = c.ApplicationUser!.Email,
                    FirstName = c.ApplicationUser.FirstName,
                    LastName = c.ApplicationUser.LastName
                }
            })
            .FirstOrDefaultAsync(cancellationToken);

        if (customerData is null)
            return Result.Failure(CustomerErrors.CustomerNotFound);

        using var transaction = await _context.Database.BeginTransactionAsync(cancellationToken);

        try
        {
            await _context.Users
                .Where(u => _context.Customers.Any(c => c.Id == request.CustomerId && c.ApplicationUser!.Id == u.Id))
                .ExecuteUpdateAsync(setters => setters.SetProperty(u => u.IsDeleted, true), cancellationToken);

            customerData.Customer.IsDeleted = true;
            await _context.SaveChangesAsync(cancellationToken);

            await transaction.CommitAsync(cancellationToken);
        }
        catch
        {
            await transaction.RollbackAsync(cancellationToken);
            throw;
        }

        try
        {
            await _emailHelper.SendAccountDeletedEmail(
                customerData.UserInfo.Email!,
                $"{customerData.UserInfo.FirstName} {customerData.UserInfo.LastName}",
                request.Reason
            );
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to send deletion email to customer: {Email} for CustomerId: {CustomerId}", customerData.UserInfo.Email, request.CustomerId);
        }

        return Result.Success();
    }
}