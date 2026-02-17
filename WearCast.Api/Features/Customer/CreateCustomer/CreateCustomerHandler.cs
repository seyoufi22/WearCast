using WearCast.Api.Entities;

namespace WearCast.Api.Features.Customer.CreateCustomer;

public class CreateCustomerHandler : IRequestHandler<CreateCustomerRequest, Result>
{ 
    private readonly IRepository<Entities.Customer> _repository;
    private readonly IValidator<CreateCustomerRequest> _validator;
    private readonly IMapper _mapper;
    private readonly UserManager<ApplicationUser> _userManager;
    
    public CreateCustomerHandler(IRepository<Entities.Customer> repository, IValidator<CreateCustomerRequest> validator, IMapper mapper, UserManager<ApplicationUser> userManager)
    {
        _repository = repository;
        _validator = validator;
        _mapper = mapper;
        _userManager = userManager;
    }

    public async Task<Result> Handle(CreateCustomerRequest request, CancellationToken cancellationToken)
    {
        var user = await _userManager.FindByEmailAsync(request.Email);
        if (user is null)
            return Result.Failure(UserErrors.UserNotFound);

        // Assign Customer Role
        if (!await _userManager.IsInRoleAsync(user, DefaultRoles.Customer))
        {
            var roleResult = await _userManager.AddToRoleAsync(user, DefaultRoles.Customer);
            if (!roleResult.Succeeded)
                return Result.Failure(new Error("User.RoleAssignmentFailed", "Failed to assign Customer role", StatusCodes.Status500InternalServerError));
        }

        var customer = _mapper.Map<Entities.Customer>(user);
        
        await _repository.CreateAsync(customer);
        return Result.Success();
    }
}