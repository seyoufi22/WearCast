using WearCast.Api.Common.Views;

namespace WearCast.Api.Features.Customers.GetAllCustomers;

public record GetAllCustomersRequest(
    string? SearchTerm = null,
    int PageIndex = 1,
    int PageSize = 10
) : IRequest<Result<PagingViewModel<GetAllCustomersResponse>>>;