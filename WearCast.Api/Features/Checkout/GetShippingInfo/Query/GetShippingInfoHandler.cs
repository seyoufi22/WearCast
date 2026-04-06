using WearCast.Api.Abstractions;
using WearCast.Api.Entities.BusinessActors;
using WearCast.Api.Entities.Identity;
using WearCast.Api.Features.Checkout.GetShippingInfo.DTOs;
using WearCast.Api.Persistence;

namespace WearCast.Api.Features.Checkout.GetShippingInfo.Query;

public class GetShippingInfoHandler : IRequestHandler<GetShippingInfoRequestDto, Result<GetShippingInfoResponseDto>>
{
    private readonly ApplicationDbContext _dbContext;

    public GetShippingInfoHandler(ApplicationDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<Result<GetShippingInfoResponseDto>> Handle(GetShippingInfoRequestDto request, CancellationToken cancellationToken)
    {
        var customer = await _dbContext.Customers
            .Include(c => c.ApplicationUser)
            .FirstOrDefaultAsync(c => c.Id == request.CustomerId, cancellationToken);

        if (customer is null)
        {
            return Result.Failure<GetShippingInfoResponseDto>(
                new Error("Checkout.CustomerNotFound", "Customer not found.", StatusCodes.Status404NotFound));
        }

        var user = customer.ApplicationUser;

        var response = new GetShippingInfoResponseDto
        {
            RecipientName = $"{user?.FirstName} {user?.LastName}".Trim(),
            PhoneNumber = user?.PhoneNumber ?? string.Empty,
            AdditionalPhoneNumber = null,
            State = customer.Address.State,
            City = customer.Address.City,
            Street = customer.Address.Street,
            BuildingNumber = customer.Address.BuildingNumber
        };

        return Result.Success(response);
    }
}
