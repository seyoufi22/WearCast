using WearCast.Api.Common.Views;

namespace WearCast.Api.Features.DesignedProductManagement.CustomerDesigns.GetAllCustomerDesigns
{
    public record GetAllCustomerDesignsRequest(
        string? SearchTerm = null,
        int PageIndex = 1,
        int PageSize = 10
        ) : IRequest<Result<PagingViewModel<GetAllCustomerDesignsResponse>>>;
}
