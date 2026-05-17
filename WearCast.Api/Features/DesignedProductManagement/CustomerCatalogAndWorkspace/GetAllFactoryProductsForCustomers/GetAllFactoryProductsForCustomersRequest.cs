using WearCast.Api.Common.Views;

namespace WearCast.Api.Features.DesignedProductManagement.CustomerCatalogAndWorkspace.GetAllFactoryProductsForCustomers
{
    public record GetAllFactoryProductsForCustomersRequest(
       string? SearchTerm = null,
       int? CategoryId = null,
       decimal? MinPrice = null,
       decimal? MaxPrice = null,
       DressStyle? DressStyle = null,
       TargetAudience? TargetAudiences = null,
       SortBy SortBy = SortBy.Newest,
       int PageIndex = 1,
       int PageSize = 20
       ) : IRequest<Result<PagingViewModel<GetAllFactoryProductsForCustomersResponse>>>;
}
