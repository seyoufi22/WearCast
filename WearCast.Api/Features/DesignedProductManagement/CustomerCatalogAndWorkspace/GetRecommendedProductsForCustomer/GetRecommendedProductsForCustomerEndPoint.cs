using WearCast.Api.Common.Views;
using Carter;
using MediatR;

namespace WearCast.Api.Features.DesignedProductManagement.CustomerCatalogAndWorkspace.GetRecommendedProductsForCustomer
{
    public class GetRecommendedProductsForCustomerEndPoint : ICarterModule
    {
        public void AddRoutes(IEndpointRouteBuilder app)
        {
            app.MapGet("api/customer-catalog/recommendations", 
                async (int? topK, ISender sender) =>
            {
                var request = new GetRecommendedProductsForCustomerRequest { TopK = topK ?? 10 };
                var result = await sender.Send(request);

                return result.IsSuccess 
                    ? Results.Ok(result.Value) 
                    : Results.BadRequest(result.Error);
            })
            .WithName("GetRecommendedProductsForCustomer")
            .WithTags("Customer Catalog")
            .RequireAuthorization();
        }
    }
}
