namespace WearCast.Api.Features.Customers.DeleteCustomer;

public record DeleteCustomerRequest(
    int CustomerId,
    string Reason
) : IRequest<Result>;

public record DeleteCustomerBody(
    string Reason
);