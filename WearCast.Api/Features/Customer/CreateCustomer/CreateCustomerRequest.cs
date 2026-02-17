namespace WearCast.Api.Features.Customer.CreateCustomer;

public record CreateCustomerRequest : IRequest<Result>
{
    public string Email { get; set; } = string.Empty;
}