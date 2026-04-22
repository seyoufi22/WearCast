namespace WearCast.Api.Features.Customers.GetCustomer;

public record GetCustomerRequest(
    int? ProvidedCustomerId = null
) : IRequest<Result<GetCustomerResponse>>;