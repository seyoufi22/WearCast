using WearCast.Api.Features.AuthenticationManagement;

namespace WearCast.Api.Features.Customers.UpdateCustomer
{
    public class UpdateCustomerHandler(
        ApplicationDbContext context,
        IHttpContextAccessor httpContextAccessor,
        IMapper mapper
        ) : IRequestHandler<UpdateCustomerRequest, Result>
    {
        private readonly ApplicationDbContext _context = context;
        private readonly IHttpContextAccessor _httpContextAccessor = httpContextAccessor;
        private readonly IMapper _mapper = mapper;
        public async Task<Result> Handle(UpdateCustomerRequest request, CancellationToken cancellationToken)
        {
            var user = _httpContextAccessor.HttpContext!.User;

            int targetCustomerId;

            if (user.IsSuperAdmin() || user.IsCustomerServiceAdmin())
            {
                if (!request.ProvidedCustomerId.HasValue)
                {
                    return Result.Failure(new Error("Validation.MissingId", "SuperAdmin must provide a target CustomerId to delete.", StatusCodes.Status400BadRequest));
                }

                targetCustomerId = request.ProvidedCustomerId.Value;
            }
            else
            {
                targetCustomerId = user.GetCustomerId()!.Value;
            }

            var customer = await _context.Customers
                .Include(x => x.ApplicationUser)
                .FirstOrDefaultAsync(x => x.Id == targetCustomerId, cancellationToken);

            if (customer == null || customer.ApplicationUser == null)
            {
                return Result.Failure(CustomerErrors.CustomerNotFound);
            }

            bool isPhoneNumberDuplicated = await _context.Users
                .AnyAsync(x => x.Id != customer.ApplicationUser.Id && x.PhoneNumber == request.PhoneNumber, cancellationToken);

            if (isPhoneNumberDuplicated)
            {
                return Result.Failure(UserErrors.DublicatedPhoneNumber);
            }

            _mapper.Map(request, customer);

            await _context.SaveChangesAsync(cancellationToken);

            return Result.Success();
        }
    }
}
